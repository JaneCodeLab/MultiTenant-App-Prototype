
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiService.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public readonly string _defaultTimeZone = GeneralVariables.DefaultTimeZone;
        public static Language _defaultLanguage = Language.English;
        public int _tenantId = 0;


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var requestHeader = request.Headers;
            if (!requestHeader.TryGetValue("tenant-id", out var tenantId))
            {
                context.Result = new BadRequestObjectResult("tenant-id cannot be null.");
                return;
            }
            else if (!int.TryParse(tenantId, out int intTenantId))
            {
                context.Result = new BadRequestObjectResult("tenant-id is not valid!");
                return;
            }
            else
                _tenantId = intTenantId;

            if (!requestHeader.TryGetValue("language", out var language))
                _defaultLanguage = Language.English;
            else if (language.ToString().ToLower() == "en")
                _defaultLanguage = Language.English;
            else if (language.ToString().ToLower() == "fr")
                _defaultLanguage = Language.French;
            else
                _defaultLanguage = Language.English;
        }

        public static object? GetNotFoundResult() => new { message = SysExpressionHelper.Get(_defaultLanguage, ExpressionTypes.SysHelp, ApiResponseExpression.ApiResponse_Not_Found.ToInt()) };
    }
}