using ApplicationService;
using ApplicationService.CustomModels.ApiModels;
using Microsoft.AspNetCore.Mvc;

namespace ApiService.Controllers
{
    [Route("api")]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] TokenRequest model)
        {
            var result = await _tokenService.Authenticate(model);
            if (result.success)
                return Ok(result.Data);
            else
                return BadRequest(result);
        }
    }
}
