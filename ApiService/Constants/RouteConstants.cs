namespace ApiService.Constants;

public static class RouteConstants
{
    public const string ApiBase = "api/v{version:apiVersion}";

    // Authentication and Authorization
    public const string Token = $"{ApiBase}/auth";

    // Task Management
    public const string Tasks = $"{ApiBase}/tasks";
    public const string TaskById = "{id:guid}";
}
