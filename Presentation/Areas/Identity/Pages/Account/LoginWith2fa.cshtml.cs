
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using ApplicationCore;
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Areas.Identity.Pages.Account
{
    public class LoginWith2faModel : PageModel
    {
        private readonly SignInManager<SysCustomUser> _signInManager;
        private readonly UserManager<SysCustomUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<LoginWith2faModel> _logger;

        public LoginWith2faModel(
            SignInManager<SysCustomUser> signInManager,
            UserManager<SysCustomUser> userManager,
            IEmailSender emailSender,
            ILogger<LoginWith2faModel> logger,
            ISysExpressionService languageSettingService, ISysTenantService tenantService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
            if (SysExpressionHelper.Items == null || !SysExpressionHelper.Items.Any())
                languageSettingService.FetchAllExpressions();

            if (TenantHelper.Tenants == null || !TenantHelper.Tenants.Any())
                tenantService.FetchHelperTenants();
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }
        public string FullName { get; set; }
        public string ProfileImage { get; set; }
        public Language UserLanguage { get; set; }
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Authenticator code")]
            public string TwoFactorCode { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember this machine")]
            public bool RememberMachine { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");


            var replacements = new Dictionary<string, object>()
            {   { ExpressionParamConstants.Date, DateTime.Now.ToString("dd/MM/yyyy") },
                { ExpressionParamConstants.Token,token }
            };
            var twoFactorEmailBody = SysExpressionHelper.Get(user.Language, ExpressionTypes.SysCustomUser, SysCustomUserExpression.CustomUser_2FEmailBody.ToInt(), replacements);


            var twoFactorEmailGreeting = SysExpressionHelper.Get(user.Language,
                                                                 ExpressionTypes.SysCustomUser,
                                                                 SysCustomUserExpression.CustomUser_2FEmailGreeting.ToInt(),
                                                                 (ExpressionParamConstants.Fullname, $"{user.FirstName} {user.LastName}"));

            var twoFactorEmailSignature = SysExpressionHelper.Get(user.Language, ExpressionTypes.SysCustomUser, SysCustomUserExpression.CustomUser_2FEmailSignature.ToInt());

            var emailBody = string.Format(Template.GetEmailTemplate(), twoFactorEmailGreeting, twoFactorEmailBody, twoFactorEmailSignature, GeneralVariables.ApplicationName);
            await _emailSender.SendEmailAsync(user.Email, SysExpressionHelper.Get(user.Language,
                                                                                  ExpressionTypes.SysCustomUser,
                                                                                  SysCustomUserExpression.CustomUser_2FEmailSubject.ToInt()), emailBody);

            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            ReturnUrl = returnUrl;
            RememberMe = rememberMe;
            ProfileImage = user.ProfileImage ?? "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAMAAABHPGVmAAAAAXNSR0IB2cksfwAAAAlwSFlzAAALEwAACxMBAJqcGAAAActQTFRF5Obn5efo4+Xm3+Lj3N/g2t3e2tze293f3d/h4OPk5Obm5ujp297fxMjKuL7As7i7r7W4rbO2rbS3sLa5tLq8u8DDzNDSyc3PtLq9rLK1rrS3vMHD0dXW4ePkx8vOsre609fZz9LUsbe6uL2/3d/g4uTlvsPGzdHS3uDhwsbJ5OXm1tnbur/CwMTHs7m8rrO2vsPFuL3AzM/R5ebn2NvcrbO3u8DC0NTVsbe5tLm8yczP2dzd4ePl09bY3+Hiv8TG0tXXw8jKt72/xsrM3uHivMHE1dna5ufoy8/QrLK24+bmyMzOx8vNys7QztLU1djZs7i83uDit7y/w8fJ0tbX0dTW4OLj5ufp19rcsba5ur/B19nb5Ofn2dvdtry+ys3Psri7vMDDr7a44uXm1NfZu8HDy8/Rtru+sLW43eDhxsvNr7S3ub7B5ujowcbH297g5efn2Nvd2dze3+Hj5Ofo1NfYv8THtbu9q7G0qrG0q7K1q7G10dXXtbq9sLa44+bn4+Xltbu+r7W3v8PGz9PV5OXn4eTlxcnL3N7gwcbI4+Xn5efp0NPVxcrM4uTm4eLk1tnawsfJ1djaxMnLt73A4uXlwMXHrrO3ztHTD7SJEwAAA4JJREFUeJztmelfEkEYgGc4LAWFXTxivILC8CINTM3yhMy8SpPQ0qxI8yrtpEPL7vvO+nNbEJQ4duedne3XB55vfHp+77zHzjsglCNHjhyZwTq9wZi3Z29+AdbIYDIXFlmsgoRoKy4pzefvwfqyfXayiyQrr+DsqKwSSBpC9X6OCpPDma6IaQ4c5OVw1WQII8EhPqlxW7IrpGBq6zg46rMcVQKxoVK1w2yTd0g06lQ63B5FBzncpDIvzcoOKS9H1Ci8ZTJ1lYxRhcTXQucgReyOgqOUDiKWMUtaaR2EtHkZHbidXkKOMVZYB2XWYziPswVSDnAQ4QSTpBPiIKTLxCLphkk8LCMM98AkhOW8cC/MIfYxSPwKIz6NAEMRnwQ6iIch8w5Il8ToBzt0p6CSgdNwyaAIlNjhNawbAkvy/9NIhmk/WDsSeE7QCLi6GPpkFOo4w9AnZ/9Fx+MxmKOFZXbhcZhEOMcgQRMwiSXIIvHDJDXnWSSY+tYVhe20EApBOmWScYHAsttPCkOsN3sHvcPGqJDm1xS1ZJBZgi7QZmVMxbJ1kfajMs3ukJihUQiXVDmQYZZCclntLm+8ouhouqrSgVBYacduZhpaKRgmZfPRpXaL3yb/mkwlD3NRSJjmshyZMJ/H8WktWGjP4LheweeoEuCFxdlkz5IoLI/wVcRYCd2YcZLoG6RgnV0edDOtb8p4kcttnPZ13NTrNHlN1a1i04ouuGDsCIXWbhluu6Sfd3gKMAqa7w7fu2+NPIg8tEd5FImIlsDQ445OLiHhytBczfpGlkYRrJ6SqrAerbILnuCn4Z7NAWFJruOfEXv18xdB1ip4WT8l0H60hGLHK4aDM7wuBl3rBWdXCNY32DxuhRjinkCYPhr8JjAAV8Q065Qar/8tQxQ7NPhownBArnSZ6HunFIaxCLzFpeGseC8r+QBdrzLz8ZNMHOPqw9hm/nM2hx74+CTH5JfM43Nhk59DuoGHM1ncyn8vgBC60x2Gar4OQsTW1OEcXOftIGQjpS/rSvg7CJk1JzvwV161+zfzyRLqZQdKYVJCOBfWLi3fdg7ru1YOQn78jEt8aka7AkJ8Y8WaVFaC+O7tZ/wMUlIa/Y8Ib2nqIG3Rsd+pYUaixP69mfilrYTU9iNTo8YO0u5CLg0mYwqjaE3jlEhJKUO/tRpbuzSiReiTPJxetGXXHNsf8nUnMnQs1OYAAAAASUVORK5CYII=";
            FullName = user.FirstName + " " + user.LastName;
            UserLanguage = user.Language;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }
            ProfileImage = user.ProfileImage ?? "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAMAAABHPGVmAAAAAXNSR0IB2cksfwAAAAlwSFlzAAALEwAACxMBAJqcGAAAActQTFRF5Obn5efo4+Xm3+Lj3N/g2t3e2tze293f3d/h4OPk5Obm5ujp297fxMjKuL7As7i7r7W4rbO2rbS3sLa5tLq8u8DDzNDSyc3PtLq9rLK1rrS3vMHD0dXW4ePkx8vOsre609fZz9LUsbe6uL2/3d/g4uTlvsPGzdHS3uDhwsbJ5OXm1tnbur/CwMTHs7m8rrO2vsPFuL3AzM/R5ebn2NvcrbO3u8DC0NTVsbe5tLm8yczP2dzd4ePl09bY3+Hiv8TG0tXXw8jKt72/xsrM3uHivMHE1dna5ufoy8/QrLK24+bmyMzOx8vNys7QztLU1djZs7i83uDit7y/w8fJ0tbX0dTW4OLj5ufp19rcsba5ur/B19nb5Ofn2dvdtry+ys3Psri7vMDDr7a44uXm1NfZu8HDy8/Rtru+sLW43eDhxsvNr7S3ub7B5ujowcbH297g5efn2Nvd2dze3+Hj5Ofo1NfYv8THtbu9q7G0qrG0q7K1q7G10dXXtbq9sLa44+bn4+Xltbu+r7W3v8PGz9PV5OXn4eTlxcnL3N7gwcbI4+Xn5efp0NPVxcrM4uTm4eLk1tnawsfJ1djaxMnLt73A4uXlwMXHrrO3ztHTD7SJEwAAA4JJREFUeJztmelfEkEYgGc4LAWFXTxivILC8CINTM3yhMy8SpPQ0qxI8yrtpEPL7vvO+nNbEJQ4duedne3XB55vfHp+77zHzjsglCNHjhyZwTq9wZi3Z29+AdbIYDIXFlmsgoRoKy4pzefvwfqyfXayiyQrr+DsqKwSSBpC9X6OCpPDma6IaQ4c5OVw1WQII8EhPqlxW7IrpGBq6zg46rMcVQKxoVK1w2yTd0g06lQ63B5FBzncpDIvzcoOKS9H1Ci8ZTJ1lYxRhcTXQucgReyOgqOUDiKWMUtaaR2EtHkZHbidXkKOMVZYB2XWYziPswVSDnAQ4QSTpBPiIKTLxCLphkk8LCMM98AkhOW8cC/MIfYxSPwKIz6NAEMRnwQ6iIch8w5Il8ToBzt0p6CSgdNwyaAIlNjhNawbAkvy/9NIhmk/WDsSeE7QCLi6GPpkFOo4w9AnZ/9Fx+MxmKOFZXbhcZhEOMcgQRMwiSXIIvHDJDXnWSSY+tYVhe20EApBOmWScYHAsttPCkOsN3sHvcPGqJDm1xS1ZJBZgi7QZmVMxbJ1kfajMs3ukJihUQiXVDmQYZZCclntLm+8ouhouqrSgVBYacduZhpaKRgmZfPRpXaL3yb/mkwlD3NRSJjmshyZMJ/H8WktWGjP4LheweeoEuCFxdlkz5IoLI/wVcRYCd2YcZLoG6RgnV0edDOtb8p4kcttnPZ13NTrNHlN1a1i04ouuGDsCIXWbhluu6Sfd3gKMAqa7w7fu2+NPIg8tEd5FImIlsDQ445OLiHhytBczfpGlkYRrJ6SqrAerbILnuCn4Z7NAWFJruOfEXv18xdB1ip4WT8l0H60hGLHK4aDM7wuBl3rBWdXCNY32DxuhRjinkCYPhr8JjAAV8Q065Qar/8tQxQ7NPhownBArnSZ6HunFIaxCLzFpeGseC8r+QBdrzLz8ZNMHOPqw9hm/nM2hx74+CTH5JfM43Nhk59DuoGHM1ncyn8vgBC60x2Gar4OQsTW1OEcXOftIGQjpS/rSvg7CJk1JzvwV161+zfzyRLqZQdKYVJCOBfWLi3fdg7ru1YOQn78jEt8aka7AkJ8Y8WaVFaC+O7tZ/wMUlIa/Y8Ib2nqIG3Rsd+pYUaixP69mfilrYTU9iNTo8YO0u5CLg0mYwqjaE3jlEhJKUO/tRpbuzSiReiTPJxetGXXHNsf8nUnMnQs1OYAAAAASUVORK5CYII=";
            FullName = user.FirstName + " " + user.LastName;
            UserLanguage = user.Language;
            ReturnUrl = returnUrl;
            RememberMe = rememberMe;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);
            var result = await _signInManager.TwoFactorSignInAsync("Email", authenticatorCode, rememberMe, Input.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
                return RedirectToAction("Index", "ChooseTenant", new { ReturnUrl = returnUrl });
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return RedirectToPage("./Lockout");
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
                ModelState.AddModelError(string.Empty, SysExpressionHelper.Get(user.Language, ExpressionTypes.SysCustomUser, SysCustomUserExpression.CustomUser_InvalidACMessage.ToInt()));
                return Page();
            }
        }
    }
}
