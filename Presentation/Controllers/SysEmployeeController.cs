
using ApplicationCore;
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace Presentation.Controllers;

[Authorize(Roles = Roles.Admin)]
public class SysEmployeeController : BaseController
{
    private readonly ILogger<SysCustomUser> _logger;
    private readonly ISysCustomUserService _userService;
    private readonly SignInManager<SysCustomUser> _signInManager;
    private readonly UserManager<SysCustomUser> _userManager;
    private readonly IEmailSender _emailSender;
    public SysEmployeeController(ILogger<SysCustomUser> logger,
                                    IDataProtectionProvider dataProtectionProvider,
                                    ISysCustomUserService userService,
                                    UserManager<SysCustomUser> userManager,
                                    SignInManager<SysCustomUser> signInManager,
                                    IEmailSender emailSender)
                        : base(dataProtectionProvider)
    {
        _userService = userService;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _userService.GetEmployeeUserListAsync());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new SysCustomUser());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SysCustomUser model)
    {
        if (ModelState.IsValid)
        {
            model.Id = Guid.NewGuid().ToString();
            model.CreatedBy = User.GetLoggedInFullName();
            model.IsEmployeeUser = true;

            var password = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6).ToUpper() + "@" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6).ToLower();
            var result = await _userManager.CreateAsync(model, password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(model);
                await SendActivationEmail(model, code, password);
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var model = await _userService.FindAsync(id);
        return View(new CustomUserEditModel
        {
            Id = model.Id,
            FirstName = model.FirstName,
            Active = model.Active,
            Email = model.Email,
            Language = model.Language,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CustomUserEditModel model)
    {
        if (ModelState.IsValid)
        {
            bool emailFlag = false;
            var oldModel = await _userService.FindAsync(model.Id);
            if (model.Email != oldModel.Email)
            {
                emailFlag = true;
                oldModel.Email = model.Email;
                oldModel.NormalizedEmail = model.Email.ToUpper();
                oldModel.UserName = model.Email;
                oldModel.NormalizedUserName = model.Email.ToUpper();
                oldModel.EmailConfirmed = false;
            }
            if (model.PhoneNumber != oldModel.PhoneNumber)
            {
                oldModel.PhoneNumber = model.PhoneNumber;
                oldModel.PhoneNumberConfirmed = false;
            }
            if (model.FirstName != oldModel.FirstName)
                oldModel.FirstName = model.FirstName;
            if (model.LastName != oldModel.LastName)
                oldModel.LastName = model.LastName;
            if (model.Language != oldModel.Language)
                oldModel.Language = model.Language;

            await _userService.Update(oldModel, User.GetLoggedInFullName());

            if (emailFlag)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(oldModel);
                await SendActivationEmail(oldModel, code, String.Empty);
            }

            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    private async Task SendActivationEmail(SysCustomUser model, string code, string password)
    {
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var returnUrl = Url.Content("~/");
        var callbackUrl = Url.Page(
            "/Account/ConfirmEmail",
            pageHandler: null,
            values: new { area = "Identity", userId = model.Id, code, returnUrl },
            protocol: Request.Scheme);


        var replacements = new Dictionary<string, object>()
        {
            { ExpressionParamConstants.CallbackUrl, HtmlEncoder.Default.Encode(callbackUrl)},
            { ExpressionParamConstants.Password, password }
        };

        var activationEmailBody = SysExpressionHelper.Get(model.Language, ExpressionTypes.SysCustomUser, SysCustomUserExpression.CustomUser_ActivationEmailBody.ToInt(), replacements);


        var emailGreeting = SysExpressionHelper.Get(model.Language, ExpressionTypes.SysCustomUser, SysCustomUserExpression.CustomUser_2FEmailGreeting.ToInt(), (ExpressionParamConstants.Fullname, $"{model.FirstName} {model.LastName}"));
        var emailSignature = SysExpressionHelper.Get(model.Language, ExpressionTypes.SysCustomUser, SysCustomUserExpression.CustomUser_2FEmailSignature.ToInt());

        var emailBody = string.Format(Template.GetEmailTemplate(), emailGreeting, activationEmailBody, emailSignature, GeneralVariables.ApplicationName);

        await _emailSender.SendEmailAsync(model.Email,
                                          SysExpressionHelper.Get(model.Language,
                                                                  ExpressionTypes.SysCustomUser,
                                                                  SysCustomUserExpression.CustomUser_ActivationSubject.ToInt()),
                                          emailBody);
    }
}