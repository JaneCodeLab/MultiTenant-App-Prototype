
using ApplicationService.CustomModels.ApiModels;

namespace ApplicationService;

public interface ITokenService
{
    Task<ApiResponse<object>> Authenticate(TokenRequest model);
}