using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System.Diagnostics;

namespace ApiService;

public class LoggingActionFilter : IActionFilter
{
    private readonly ILogger<LoggingActionFilter> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string StopwatchKey = "ActionExecutionStopwatch";
    private const string TenantIdHeader = "tenant-id";
    private const string LanguageHeader = "language";
    private const string ControllerValue = "controller";
    private const string UnknownController = "UnknownController";
    private const string ActionValue = "action";
    private const string UnknownAction = "UnknownAction";
    private const string UnknownIP = "UnknownIP";
    private const string DefaultUser = "Anonymous";


    public LoggingActionFilter(ILogger<LoggingActionFilter> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        var stopwatch = Stopwatch.StartNew();
        httpContext.Items[StopwatchKey] = stopwatch;

        var controller = context.RouteData.Values[ControllerValue]?.ToString() ?? UnknownController;
        var action = context.RouteData.Values[ActionValue]?.ToString() ?? UnknownAction;
        var method = httpContext.Request.Method;
        var path = httpContext.Request.Path;
        var user = context.HttpContext.User.Identity?.Name ?? DefaultUser;
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? UnknownIP;

        httpContext.Request.Headers.TryGetValue(TenantIdHeader, out StringValues tenantId);
        httpContext.Request.Headers.TryGetValue(LanguageHeader, out StringValues language);

        _logger.LogInformation($"Executing {method} {path} -> {controller}.{action} | User: {user} | IP: {ip} | Tenant: {tenantId} | Language: {language}");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var httpContext = context.HttpContext;

        var controller = context.RouteData.Values[ControllerValue]?.ToString() ?? UnknownController;
        var action = context.RouteData.Values[ActionValue]?.ToString() ?? UnknownAction;
        var statusCode = context.HttpContext.Response.StatusCode;
        var stopwatch = httpContext.Items[StopwatchKey] as Stopwatch;

        stopwatch?.Stop();
        var elapsedMs = stopwatch?.ElapsedMilliseconds ?? 0;

        if (context.Exception != null)
            _logger.LogError( 
                              context.Exception,
                              "{Controller}.{Action} threw an exception after {ElapsedMs} ms. Status: {StatusCode}",
                              controller, action, elapsedMs, statusCode
                            );
        else
            _logger.LogInformation(
                                    "{Controller}.{Action} executed successfully in {ElapsedMs} ms. Status: {StatusCode}",
                                    controller, action, elapsedMs, statusCode
                                  );
    }
}
