
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Authorize]
    public class ChangeUserSettingsController : BaseController
    {
        private readonly ILogger<ChangeUserSettingsController> _logger;
        private readonly UserManager<SysCustomUser> _userManager;
        private readonly SignInManager<SysCustomUser> _signInManager;
        private readonly ISysCustomUserService _UserService;

        public ChangeUserSettingsController(ILogger<ChangeUserSettingsController> logger,
                                            IDataProtectionProvider dataProtectionProvider,
                                            UserManager<SysCustomUser> userManager,
                                            SignInManager<SysCustomUser> signInManager,
                                            ISysCustomUserService userService)
                                     : base(dataProtectionProvider)
        {
            _logger = logger;
            _UserService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> ChangeLanguage(string language, string returnUrl)
        {
            var lang = (Language)Enum.Parse(typeof(Language), language, true);

            var user = await _UserService.FindAsync(User.GetLoggedInUserId());
            user.Language = lang;

            await _UserService.Update(user, user.FirstName + " " + user.LastName);
            var claims = await _userManager.GetClaimsAsync(user);
            await _userManager.ReplaceClaimAsync(user,
                                                    claims.FirstOrDefault(c => c.Type == CustomClaimType.LanguageName),
                                                    new Claim(CustomClaimType.LanguageName, user.Language.ToString()));
            await _signInManager.RefreshSignInAsync(user);
            return Redirect(returnUrl.Replace("@", "&") ?? "~/");
        }

        public async Task<IActionResult> ChangeTimeZone(string timeZone, string returnUrl)
        {
            var user = await _UserService.FindAsync(User.GetLoggedInUserId());
            user.TimeZone = timeZone;

            await _UserService.Update(user, user.FirstName + " " + user.LastName);
            var claims = await _userManager.GetClaimsAsync(user);
            await _userManager.ReplaceClaimAsync(user,
                                                    claims.FirstOrDefault(c => c.Type == CustomClaimType.TimeZone),
                                                    new Claim(CustomClaimType.TimeZone, user.TimeZone));
            await _signInManager.RefreshSignInAsync(user);
            return Redirect(returnUrl ?? "~/");
        }
    }
}