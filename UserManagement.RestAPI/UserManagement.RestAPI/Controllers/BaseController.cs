using Microsoft.AspNetCore.Mvc;
using UserManagement.RestAPI.Models.Enums;
using UserManagement.RestAPI.Models.Misc;
using UserManagement.RestAPI.Models.ServiceResponse;

namespace UserManagement.RestAPI.Controllers;

public abstract class BaseController : ControllerBase
{
    protected IActionResult HandleServiceResult<T>(in Result<T, ServiceResponseErrorCode> result)
    {
        return result.Resolve(
            s => Ok((ServiceResponse<T>)s),
            (e, errCode) => HandleErrorResult<T>(errCode, e)
            );
    }

    protected IActionResult HandleErrorResult<T>(ServiceResponseErrorCode errCode, string error)
    {
        return errCode switch
        {
            ServiceResponseErrorCode.General => StatusCode(StatusCodes.Status500InternalServerError, new ServiceResponse<T>(error)),
            ServiceResponseErrorCode.BadRequest => StatusCode(StatusCodes.Status400BadRequest, new ServiceResponse<T>(error)),
            ServiceResponseErrorCode.NotFound => StatusCode(StatusCodes.Status404NotFound, new ServiceResponse<T>(error)),
            ServiceResponseErrorCode.Unauthorized => StatusCode(StatusCodes.Status401Unauthorized, new ServiceResponse<T>(error)),
            ServiceResponseErrorCode.Forbidden => StatusCode(StatusCodes.Status403Forbidden, new ServiceResponse<T>(error)),
            ServiceResponseErrorCode.Timeout => StatusCode(StatusCodes.Status408RequestTimeout, new ServiceResponse<T>(error)),
            _ => StatusCode(StatusCodes.Status500InternalServerError, new ServiceResponse<T>(error)),
        };
    }
}
