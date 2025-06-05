
using ApplicationCore.DomainModel;
using ApplicationService.CustomModels.ApiModels;
using Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApplicationService;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly ISysApiUserService _apiUserService;
    public TokenService(IConfiguration configuration, ISysApiUserService apiUserService)
    {
        _configuration = configuration;
        _apiUserService = apiUserService;
    }

    public async Task<ApiResponse<object>> Authenticate(TokenRequest model)
    {
        var apiUserResult = await _apiUserService.GetAsync(model.Email, model.Password.Hash(), model.UserName, Language.English);
        if (!apiUserResult.success)
            return new ApiResponse<object>(false, apiUserResult.Explanation);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims: new[]
                {
                    new Claim(ClaimTypes.Name, model.Email),
                    new Claim(ClaimTypes.Role, "ApiUser"),
                },
            expires: DateTime.UtcNow.AddMinutes(120),
            signingCredentials: signIn);

        var issuer = _configuration.GetValue("Jwt:Issuer", string.Empty);

        return new ApiResponse<object>(true,
                                          String.Empty,
                                          new { Token = new JwtSecurityTokenHandler().WriteToken(token), Expiration = DateTime.UtcNow.AddMinutes(120) });
    }
}