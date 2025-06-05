
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize]
    public class SysAssistanceController : BaseController
    {
        private readonly ILogger<SysAssistanceController> _logger;
        private readonly ISysHelpService _helpService;
        private readonly ISysFaqService _faqService;

        public SysAssistanceController(ILogger<SysAssistanceController> logger,
                                    IDataProtectionProvider dataProtectionProvider,
                                    ISysHelpService helpService,
                                    ISysFaqService faqService)
                            : base(dataProtectionProvider)
        {
            _logger = logger;
            _helpService = helpService;
            _faqService = faqService;
        }

        [HttpGet]
        public async Task<IActionResult> Help(string controllerName, string actionName, string id)
        {
            var lang = User.GetLoggedInUserLanguageEnum();
            try
            {
                //ToDo: Handle It
                //var TitleAppKeyword = (AppKeyword)Enum.Parse(typeof(AppKeyword), controllerName);
                //ViewBag.pageTitle = SysExpressionHelper.Get(lang,ExpressionTypes.General,GeneralExpression.TitleAppKeyword.ToInt());
            }
            catch
            {
                ViewBag.pageTitle = controllerName;
            }
            ViewBag.controllerName = controllerName;
            ViewBag.langEnum = lang;
            ViewBag.returnUrl = $"/{controllerName}/{actionName}";
            if (!string.IsNullOrEmpty(id)) ViewBag.returnUrl = $"/{controllerName}/{actionName}/{id}";

            var help = await _helpService.GetHelpAsync(controllerName, actionName, lang);
            if (string.IsNullOrEmpty(help))
            {
                ViewBag.help = "There is not help document for this page.";
                //Todo: Send email to songul
            }
            else
            {
                ViewBag.help = help;
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Faq(string controllerName, string actionName, string id)
        {
            var lang = User.GetLoggedInUserLanguageEnum();
            try
            {
                //ToDo: Handle It
                //var TitleAppKeyword = (AppKeyword)Enum.Parse(typeof(AppKeyword), controllerName);
                //ViewBag.pageTitle = SysExpressionHelper.Get(TitleAppKeyword, lang);
            }
            catch
            {
                ViewBag.pageTitle = controllerName;
            }
            ViewBag.controllerName = controllerName;
            ViewBag.langEnum = lang;
            ViewBag.returnUrl = $"/{controllerName}/{actionName}";
            if (!string.IsNullOrEmpty(id)) ViewBag.returnUrl = $"/{controllerName}/{actionName}/{id}";

            var faqs = await _faqService.GetFaqsAsync(controllerName, actionName, lang);

            return View(faqs);
        }
    }
}
