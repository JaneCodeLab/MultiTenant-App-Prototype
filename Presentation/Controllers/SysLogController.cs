
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.SqlServerAdapter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize(Roles = Roles.DevPersonnel + "," + Roles.Admin)]
public class SysLogController : BaseController
{
    private readonly ILogger<SysLogController> _logger;
    private readonly ISysLogService _logService;

    public SysLogController(ILogger<SysLogController> logger,
                            IDataProtectionProvider dataProtectionProvider,
                                ISysLogService logService,
                                IServiceProvider serviceProvider)
                            : base(dataProtectionProvider, serviceProvider)
    {
        _logger = logger;
        _logService = logService;
    }

    public async Task<IActionResult> Index(BaseFilter filter)
    {
        filter = GetLatestFilter(filter);

        var model = new ViewListModel<BaseFilter>(filter);
        return View(model);
    }


    [HttpPost, ActionName(nameof(GetData))]
    public async Task<IActionResult> GetData() => await base.GetPagedData<SysLog, Guid, BaseFilter, SysLogMinimalListItem, ApplicationDbContext>(_logService,
        a => a.ToDto(User.GetLoggedInUserTimezoneId()));


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
