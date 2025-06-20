
using ApplicationCore.DomainModel;
using ApplicationService.Constants;
using ApplicationService.CustomModels.ApiModels;
using Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApplicationService;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ISysApiUserService _apiUserService;

    public TokenService(IOptions<JwtSettings> jwtOptions, ISysApiUserService apiUserService)
    {
        _jwtSettings = jwtOptions?.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
        _apiUserService = apiUserService ?? throw new ArgumentNullException(nameof(apiUserService));
    }

    public async Task<ApiResponse<object>> Authenticate(TokenRequest model)
    {
        var apiUserResult = await _apiUserService.GetAsync(model.Email, model.Password.Hash(), model.UserName, Language.English);
        if (!apiUserResult.Success)
            return ApiResponse<object>.Fail(ApiCommonMessages.AuthenticationFailed);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddMinutes(120);

        var user = apiUserResult.Data;
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "ApiUser")
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
             claims: claims,
             expires: expiration,
             signingCredentials: credentials
         );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return ApiResponse<object>.Ok(
            new { Token = tokenString, Expiration = expiration },
            message: string.Empty
        );
    }
}