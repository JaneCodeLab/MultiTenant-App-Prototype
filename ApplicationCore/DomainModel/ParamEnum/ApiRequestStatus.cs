
namespace ApplicationCore.DomainModel;

public enum ApiRequestStatus
{
    OK = 200,
    BadRequest = 400,
    Unauthorized = 401,
    InternalServerError = 500,
    NotImplemented = 501,
    BadGateway = 502,
    ServiceUnavailable = 503,
    GatewayTimeout = 504,
}