
namespace ApplicationService;

public record ServiceResult<T>(T Result, bool IsSucceed = true, string? Explanation = null);