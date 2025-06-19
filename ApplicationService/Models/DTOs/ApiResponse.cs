
namespace ApplicationService;

public record ApiResponse<T>
{
    public bool Success { get; init; }
    public string Explanation { get; init; } = string.Empty;
    public T? Data { get; init; }

    public static ApiResponse<T> Ok(T data, string? message = null) => new()
    {
        Success = true,
        Explanation = message,
        Data = data
    };

    public static ApiResponse<T> Fail(string error) => new()
    {
        Success = false,
        Explanation = error,
        Data = default
    };
}