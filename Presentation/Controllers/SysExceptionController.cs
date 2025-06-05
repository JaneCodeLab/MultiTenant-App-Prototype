
using ApplicationCore.DomainModel;
using ApplicationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize(Roles = Roles.DevPersonnel + "," + Roles.Admin)]
public class SysExceptionController : BaseController
{
    private readonly ILogger<SysExceptionController> _logger;
    private readonly ISysExceptionService _exceptionService;

    public SysExceptionController(ILogger<SysExceptionController> logger,
                                  IDataProtectionProvider dataProtectionProvider,
                                  ISysExceptionService exceptionService)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _exceptionService = exceptionService;
    }

    public async Task<IActionResult> Index(BaseFilter filter)
    {
        filter = GetLatestFilter(filter);
        if (filter.CreatedAtStart == null)
            filter.CreatedAtStart = DateTime.SpecifyKind(DateTime.Now.Date.AddDays(-2), DateTimeKind.Unspecified);

        var model = new ViewListModel<SysException, BaseFilter>
        {
            Records = await _exceptionService.GetAllAsync(filter, User.GetLoggedInUserTimezoneId()),
            Filter = filter
        };

        return View(model);
    }

    public async Task<IActionResult> Detail(Guid id)
    {
        var model = await _exceptionService.GetAsync(id);
        return View(model);
    }

    public async Task<IActionResult> ChangeState(Guid id, bool activate)
    {
        await _exceptionService.ChangeActivateAsync(id, !activate, User.GetOnlineUser());
        return RedirectToAction(nameof(Index));
    }
}