
namespace ApplicationService;

public record CreateObjectResult<T>(bool Success, T? ObjectId, bool isNewRecord = false);