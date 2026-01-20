namespace UserManagement.RestAPI.Models.Enums;

public enum ServiceResponseErrorCode
{
    None,
    General,
    BadRequest,
    NotFound,
    Forbidden,
    Unauthorized,
    Timeout
}
