
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers;

public class ErrorsController : Controller
{
    private readonly ISysExceptionService _sysExceptionService;
    public ErrorsController(ISysExceptionService sysExceptionService)
    {
        _sysExceptionService = sysExceptionService;
    }

    public IActionResult Index(int statusCode)
    {
        var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

        ViewBag.StatusCode = statusCode;
        ViewBag.OriginalPath = feature?.OriginalPath;
        ViewBag.OriginalQueryString = feature?.OriginalQueryString;

        return View();
    }

    [Route("Error")]
    public async Task<IActionResult> Error()
    {
        var excepetionDetail = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        var model = new SysException();
        var user = new SysCustomUser();

        try { model.ControllerName = excepetionDetail.RouteValues["controller"].ToString(); } catch { }
        try { model.ActionName = excepetionDetail.RouteValues["action"].ToString(); } catch { }
        try { model.StackTrace = excepetionDetail.Error.StackTrace; } catch { }
        try { model.Source = excepetionDetail.Error.Source; } catch { }
        try { model.Message = excepetionDetail.Error.Message; } catch { }
        try { model.Type = excepetionDetail.Error.GetType().Name; } catch { }
        try { model.InnerExceptionMessage = excepetionDetail.Error.InnerException?.Message; } catch { }
        try { model.InnerExceptionType = excepetionDetail.Error.InnerException?.GetType()?.Name; } catch { }
        try { model.Parameters = Request.QueryString.Value; } catch { }
        try { model.Path = excepetionDetail.Path; } catch { }
        try { model.Method = Request.Method; } catch { }
        try { model.IP = Request.HttpContext.Connection.RemoteIpAddress.ToString(); } catch { }
        try { model.Host = Request.Host.Value; } catch { }
        try { model.Scheme = Request?.Scheme; } catch { }
        try { model.Language = User.Claims.FirstOrDefault(x => x.Type == CustomClaimType.LanguageName)?.Value; } catch { }

        try { user.Id = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value; } catch { }
        try { user.FirstName = User.Claims.FirstOrDefault(x => x.Type == CustomClaimType.FullName)?.Value; } catch { }

        await _sysExceptionService.CreateAsync(model, user);

        if (User.Identity.IsAuthenticated)
        {
            return View(new ErrorViewModel { RequestId = model.Id.ToString() });
        }
        else
        {
            return View("ErrorPage", new ErrorViewModel { RequestId = model.Id.ToString() });
        }

    }
    public IActionResult ErrorPage(ErrorViewModel model)
    {
        return View(model);
    }
    public IActionResult Lock(string returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }
    public IActionResult AccessDenied(string returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    public IActionResult Message(string menuTitle, string message, string returnUrl)
    {
        ViewBag.MenuTitle = menuTitle;
        ViewBag.Message = message;
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }
}
