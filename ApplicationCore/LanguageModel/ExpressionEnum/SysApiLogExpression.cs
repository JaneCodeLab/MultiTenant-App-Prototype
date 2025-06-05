
namespace ApplicationCore.DomainModel;

public enum SysApiLogExpression
{
    ApiLog = 11500,
    ApiLogs = 11501,
    ApiLog_Detail = 11505,

    ApiRequestTypeParamId = 11510,
    RequestIP = 11511,
    RequestURL = 11512,
    RequestDateTime = 11513,
    ResponseDateTime = 11514,
    Duration = 11515,
    ResponseBodyCompressed = 11516,
    RequestBodyCompressed = 11517,
    ApiRequestStatusParamId = 11518,
}