
using ApplicationCore.DomainModel;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Presentation;
public class CustomSignInManager : SignInManager<SysCustomUser>
{
    public CustomSignInManager(UserManager<SysCustomUser> userManager,
                            IHttpContextAccessor contextAccessor,
                            IUserClaimsPrincipalFactory<SysCustomUser> claimsFactory,
                            IOptions<IdentityOptions> optionsAccessor,
                            ILogger<SignInManager<SysCustomUser>> logger,
                            IAuthenticationSchemeProvider schemes,
                            IUserConfirmation<SysCustomUser> confirmation)
    : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
    }
    public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
    {
        var signInResult = await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        if (signInResult.Succeeded || signInResult.RequiresTwoFactor)
            await Login(userName, signInResult);

        return signInResult;
    }

    private async Task Login(string userName, SignInResult signInResult)
    {
        var user = await UserManager.FindByNameAsync(userName);
        var claims = await UserManager.GetClaimsAsync(user);
        var roles = await UserManager.GetRolesAsync(user);
        await UserManager.RemoveClaimsAsync(user, claims);
        await UserManager.AddClaimAsync(user, new Claim(CustomClaimType.FullName, $"{user.FirstName} {user.LastName}"));
        if (!string.IsNullOrEmpty(user.ProfileImage))
            await UserManager.AddClaimAsync(user, new Claim(CustomClaimType.ProfileImage, user.ProfileImage));

        await UserManager.AddClaimAsync(user, new Claim(CustomClaimType.LanguageName, user.Language.ToString()));
        await UserManager.AddClaimAsync(user, new Claim(CustomClaimType.TimeZone, user.TimeZone));

        foreach (var item in roles)
            await UserManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, item));

        if (signInResult.Succeeded)
            await RefreshSignInAsync(user);
    }
}