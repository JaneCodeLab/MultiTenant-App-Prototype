
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
        private const string TenantIdHeader = "tenant-id";
        private const string LanguageHeader = "language";
        public readonly string _defaultTimeZone = GeneralVariables.DefaultTimeZone;

        protected int TenantId { get; private set; }
        protected Language RequestLanguage { get; private set; } = Language.English;


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var headers = context.HttpContext.Request.Headers;

            if (!headers.TryGetValue(TenantIdHeader, out var tenantIdValue) ||
                !int.TryParse(tenantIdValue, out var tenantId))
            {
                context.Result = new BadRequestObjectResult("tenant-id is required and must be a valid integer.");
                return;
            }

            TenantId = tenantId;

            // Language parsing
            if (headers.TryGetValue(LanguageHeader, out var languageValue))
            {
                RequestLanguage = languageValue.ToString().ToLower() switch
                {
                    "en" => Language.English,
                    "fr" => Language.French,
                    _ => Language.English
                };
            }
            else
            {
                RequestLanguage = Language.English;
            }

        }

        protected IActionResult FromApiResponse<T>(ApiResponse<T> response) => response.Success ? Ok(response) : BadRequest(response);
    }
}