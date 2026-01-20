namespace UserManagement.RestAPI.Models.ServiceResponse;

public class ServiceResponse<T>
{
    public T? Data { get; set; } = default!;
    public bool Success { get; set; }
    public string? Error { get; set; } = null!;
    public static implicit operator ServiceResponse<T>(T data) => new(data);

    public ServiceResponse() { }

    public ServiceResponse(string error)
    {
        Data = default!;
        Success = false;
        Error = error;
    }

    public ServiceResponse(T data)
    {
        Data = data;
        Success = true;
        Error = null;
    }
}
