
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
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace Presentation.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<SysCustomUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<SysCustomUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
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
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                var resetPasswordEmailBody = SysExpressionHelper.Get(user.Language,
                                             ExpressionTypes.SysCustomUser,
                                             SysCustomUserExpression.CustomUser_ResetPasswordEmailBody.ToInt(),
                                             (ExpressionParamConstants.CallbackUrl,
                                             HtmlEncoder.Default.Encode(callbackUrl)));

                var greeting = SysExpressionHelper.Get(user.Language,
                                             ExpressionTypes.SysCustomUser,
                                             SysCustomUserExpression.CustomUser_2FEmailGreeting.ToInt(),
                                             (ExpressionParamConstants.Fullname, $"{user.FirstName} {user.LastName}"));

                var emailSignature = SysExpressionHelper.Get(user.Language, ExpressionTypes.SysCustomUser, SysCustomUserExpression.CustomUser_2FEmailSignature.ToInt());


                var emailBody = string.Format(Template.GetEmailTemplate(), greeting, resetPasswordEmailBody, emailSignature, GeneralVariables.ApplicationName);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    SysExpressionHelper.Get(user.Language, ExpressionTypes.SysCustomUser, SysCustomUserExpression.CustomUser_ResetPasswordSubject.ToInt()),
                    emailBody);

                return RedirectToPage("./ForgotPasswordConfirmation");
            }
            return Page();
        }
    }
}
