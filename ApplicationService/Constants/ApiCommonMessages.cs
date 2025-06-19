namespace ApplicationService.Constants;

public class ApiCommonMessages
{
    public const string UnexpectedError = "An unexpected error occurred. Please try again later.";
    public const string ValidationFailed = "One or more validation errors occurred.";
    public const string Unauthorized = "You are not authorized to access this resource.";
    public const string UnhandledException = "Unhandled exception occurred.";
    public const string Forbidden = "You do not have permission to perform this action.";
    public const string InvalidTenant = "Invalid tenant ID.";
    public const string TenantMissing = "Tenant ID is missing from request headers.";
    public const string AuthenticationFailed = "Authentication failed. Please check your credentials.";

}
