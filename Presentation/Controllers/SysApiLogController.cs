
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Controllers;

[Authorize(Roles = Roles.DevPersonnel + "," + Roles.Admin)]
public class SysApiLogController : BaseController
{
    private readonly ILogger<SysApiLogController> _logger;
    private readonly ISysApiLogService _logService;

    public SysApiLogController(ILogger<SysApiLogController> logger,
                            IDataProtectionProvider dataProtectionProvider,
                            ISysApiLogService logService)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _logService = logService;
    }

    public async Task<IActionResult> Index(ApiLogFilter filter)
    {
        filter = GetLatestFilter(filter);

        if (filter.CreatedAtStart == null)
            filter.CreatedAtStart = DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Unspecified);

        if (filter.ApiRequestTypeParamId == null)
            filter.ApiRequestTypeParamId = 1;

        var model = new ViewListModel<SysApiLog, ApiLogFilter>
        {
            Records = await _logService.GetAllAsync(filter, User.GetLoggedInUserTimezoneId()),
            Filter = filter
        };
        ViewData["ApiRequestTypeList"] = new SelectList(SysParameterHelper.GetAll((Language)User.GetLoggedInUserLanguageCode(),
                                                                                   ParameterTypes.ApiRequestType),
                                         nameof(SysParameterModel.ParameterItem),
                                         nameof(SysParameterModel.Equivalent),
                                         model.Filter.ApiRequestTypeParamId);

        return View(model);
    }

    public IActionResult Sent()
    {
        return RedirectToAction(nameof(Index), new ApiLogFilter { ApiRequestTypeParamId = 1 });
    }

    public IActionResult Received()
    {
        return RedirectToAction(nameof(Index), new ApiLogFilter { ApiRequestTypeParamId = 2 });
    }

    public async Task<IActionResult> Detail(Guid id)
    {
        var model = await _logService.GetAsync(id);
        return View(model);
    }

    public async Task<IActionResult> ChangeState(Guid id, bool activate)
    {
        await _logService.ChangeActivateAsync(id, !activate, User.GetOnlineUser());
        return RedirectToAction(nameof(Index));
    }
}