using ApiService.Constants;
using ApplicationService;
using ApplicationService.CustomModels.ApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiService.Controllers
{
    [ApiController]
    [SkipSwaggerHeader]
    [ApiVersion("1.0")]
    [Route(RouteConstants.Token)]
    [ApiExplorerSettings(GroupName = "v1")]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GenerateToken([FromBody] TokenRequest model)
        {
            var result = await _tokenService.Authenticate(model);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
