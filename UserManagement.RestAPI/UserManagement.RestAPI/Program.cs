using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
using UserManagement.RestAPI.DBContext;
using UserManagement.RestAPI.Models.Constants;
using UserManagement.RestAPI.Models.ServiceResponse;
using UserManagement.RestAPI.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ServiceConfiguration config=new ServiceConfiguration();
builder.Configuration.Bind(config);
_ =builder.Services.AddSingleton(config);

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        o.JsonSerializerOptions.PropertyNamingPolicy= System.Text.Json.JsonNamingPolicy.CamelCase;
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    })
    .ConfigureApiBehaviorOptions(o =>
    {
        o.InvalidModelStateResponseFactory = context => 
        {
            var validationProblems = new ValidationProblemDetails(context.ModelState);
            return new BadRequestObjectResult(new ServiceResponse<object>(JsonSerializer.Serialize(validationProblems.Errors)));
        };
    });

var serviceConfig = (config as ServiceConfiguration)!;

_ = builder.Services.AddDbContext<UserManagementDBContext>(options =>
{
    //string? connectionString = builder.Configuration.GetConnectionString(serviceConfig.DBConnectionString);
    _ = options.UseSqlServer(serviceConfig.DBConnectionString, o =>
    {
        //Configure Automatic retry
        _ = o.EnableRetryOnFailure(ServiceConstants.MaxRetry,
            TimeSpan.FromMicroseconds(ServiceConstants.MaxRetryDelayInMilliseconds),
            null);
    });
});

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


#region It Helps to create database if not exists
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserManagementDBContext>();
    dbContext.Database.Migrate();
}
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // show detailed exceptions while debugging
    app.UseDeveloperExceptionPage();

    // log any errors during swagger setup so we can inspect runtime issues
    try
    {
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "UserManagement API V1");
        // serve swagger UI at application root
        options.RoutePrefix = string.Empty;
    });
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Error configuring Swagger UI");
        throw;
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
