
using ApplicationService.CustomModels.ApiModels;

namespace ApplicationService;

public interface ITokenService
{
    /// <summary>
    /// Authenticates a user and generates a token.
    /// </summary>
    /// <returns>An ApiResponse containing the authentication result.</returns>
    Task<ApiResponse<object>> Authenticate(TokenRequest model);
}