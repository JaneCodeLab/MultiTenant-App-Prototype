
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Controllers;

//[Authorize(Roles = Roles.DevPersonnel + "," + Roles.Admin)]
[Authorize]
public class DepartmentRoleController : BaseController
{
    private readonly ILogger<DepartmentRoleController> _logger;
    private readonly IDepartmentRoleService _departmentRoleService;
    private readonly IDepartmentService _departmentService;

    public DepartmentRoleController(ILogger<DepartmentRoleController> logger,
                             IDataProtectionProvider dataProtectionProvider,
                             IDepartmentService departmentService,
                             IDepartmentRoleService departmentRoleService)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _departmentService = departmentService;
        _departmentRoleService = departmentRoleService;
    }

    public async Task<IActionResult> Index()
    {
        var departmentId = (User.GetLoggedInUserSquad()).ToInt();

        ViewBag.DepartmentId = departmentId;
        var model = new ViewListModel<DepartmentRole, BaseFilter>
        {
            Records = await _departmentRoleService.GetDepartmentRoles(departmentId),
            Filter = new BaseFilter(),
        };
        return View(model);
    }

    [HttpPost]
    public async Task<JsonResult> Save(DepartmentRole model)
    {
        ServiceResult result;
        if (model.Id != Guid.Empty)
        {
            result = await _departmentRoleService.CustomUpdateAsync(model, User.GetOnlineUser());
        }
        else
        {
            model.Id = Guid.NewGuid();
            result = await _departmentRoleService.CreateAsync(model, User.GetOnlineUser());
        }

        return new JsonResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _departmentRoleService.PermanentDeleteAsync(User.GetOnlineUser(), id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ChangeState(Guid id, bool activate)
    {
        await _departmentRoleService.ChangeActivateAsync(id, !activate, User.GetOnlineUser());
        return RedirectToAction(nameof(Index));
    }
}