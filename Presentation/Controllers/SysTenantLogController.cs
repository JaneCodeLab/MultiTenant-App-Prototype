
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.SqlServerAdapter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize(Roles = Roles.DevPersonnel + "," + Roles.Admin)]
public class SysTenantLogController : BaseController
{
    private readonly ILogger<SysTenantLogController> _logger;
    private readonly ILogService _logService;

    public SysTenantLogController(ILogger<SysTenantLogController> logger,
                            IDataProtectionProvider dataProtectionProvider,
                                ILogService logService,
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
    public async Task<IActionResult> GetData() => await base.GetPagedData<SysLog, Guid, BaseFilter, SysLogMinimalListItem, TenantDbContext>(_logService,
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
