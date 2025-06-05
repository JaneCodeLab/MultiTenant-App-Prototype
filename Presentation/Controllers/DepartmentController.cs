
using ApplicationCore.DomainModel;
using ApplicationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

//[Authorize(Roles = $"{Roles.DevPersonnel},{Roles.Admin},{Roles.CompanyAdmin}")]
[Authorize]
public class DepartmentController : BaseController
{
    private readonly ILogger<DepartmentController> _logger;
    private readonly IDepartmentService _departmentService;

    public DepartmentController(ILogger<DepartmentController> logger,
                             IDataProtectionProvider dataProtectionProvider,
                             IDepartmentService departmentService)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _departmentService = departmentService;
    }

    public async Task<IActionResult> Index(BaseFilter filter)
    {
        filter = GetLatestFilter(filter);

        var model = new ViewListModel<Department, BaseFilter>
        {
            Records = await _departmentService.GetAllAsync(filter, User.GetLoggedInUserTimezoneId()),
            Filter = filter
        };

        return View(model);
    }

    [HttpPost]
    public async Task<JsonResult> Save(Department model)
    {
        ServiceResult result;
        model.Title = model.Title?.Trim() ?? string.Empty;
        if (model.Id > 0)
            result = await _departmentService.CustomUpdateAsync(model, User.GetOnlineUser());
        else
            result = await _departmentService.CreateAsync(model, User.GetOnlineUser());

        return new JsonResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _departmentService.PermanentDeleteAsync(User.GetOnlineUser(), id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ChangeState(int id, bool activate)
    {
        await _departmentService.ChangeActivateAsync(id, !activate, User.GetOnlineUser());
        return RedirectToAction(nameof(Index));
    }
}
