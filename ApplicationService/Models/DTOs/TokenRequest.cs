
namespace ApplicationService.CustomModels.ApiModels;

public record TokenRequest
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
}