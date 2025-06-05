
namespace ApplicationService.CustomModels.ApiModels;

public record ApiResponse<T>
(
    bool success,
    string Explanation,
    T? Data = default
);