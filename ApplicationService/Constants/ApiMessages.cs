namespace ApplicationService.Constants;

internal static class ApiMessages
{
    public static string Created(string entity) => $"{Validate(entity)} created successfully.";
    public static string Updated(string entity) => $"{Validate(entity)} updated successfully.";
    public static string Deleted(string entity) => $"{Validate(entity)} deleted successfully.";
    public static string NotFound(string entity) => $"{Validate(entity)} not found.";
    public static string Unauthorized(string entity = "resource") => $"Unauthorized access to {Validate(entity).ToLowerInvariant()}.";
    public static string FetchSuccess(string entityPlural) => $"{Validate(entityPlural)} fetched successfully.";
    public static string AlreadyExists(string entity) => $"{Validate(entity)} already exists.";
    public static string InvalidRequest(string entity) => $"Invalid {Validate(entity).ToLowerInvariant()} request.";
    public static string OperationFailed(string entity) => $"Failed to process {Validate(entity).ToLowerInvariant()}.";
    public static string ValidationFailed(string entity) => $"Validation failed for the {Validate(entity).ToLowerInvariant()}.";
    
    
    
    private static string Validate(string value) => string.IsNullOrWhiteSpace(value) ? "entity" : value;
}
