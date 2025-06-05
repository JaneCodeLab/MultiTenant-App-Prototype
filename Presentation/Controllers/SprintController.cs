
using ApplicationCore.DomainModel;
using ApplicationService;
using DocumentFormat.OpenXml.Spreadsheet;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Controllers;

[Authorize]
public class SprintController : BaseController
{
    private readonly ILogger<SprintController> _logger;
    private readonly ICustomerService _customerService;
    private readonly IDepartmentService _departmentService;
    private readonly ISprintService _sprintService;

    public SprintController(ILogger<SprintController> logger,
                             IDataProtectionProvider dataProtectionProvider,
                             IDepartmentService departmentService,
                             ICustomerService customerService,
                             ISprintService sprintService)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _sprintService = sprintService;
        _customerService = customerService;
        _departmentService = departmentService;
    }

    public async Task<IActionResult> Index()
    {
        var departmentId = (User.GetLoggedInUserSquad()).ToInt();
        ViewBag.DepartmentId = departmentId;
        return View(await _sprintService.GetDepartmentSprintsAsync(departmentId));
    }

    [HttpPost]
    public async Task<JsonResult> Save(Sprint model)
    {
        ServiceResult result;
        model.Title = model.Title?.Trim() ?? string.Empty;
        if (model.Id > 0)
            result = await _sprintService.CustomUpdateAsync(model, User.GetOnlineUser());
        else
            result = await _sprintService.CreateAsync(model, User.GetOnlineUser());

        return new JsonResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _sprintService.PermanentDeleteAsync(User.GetOnlineUser(), id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> MakeCurrent(int id)
    {
        var departmentId = (User.GetLoggedInUserSquad()).ToInt();
        var sprints = await _sprintService.GetDepartmentSprintsAsync(departmentId);
        foreach (var item in sprints)
        {
            if (!item.Current && item.Id != id)
                continue;

            item.Current = item.Id == id ? true : false;
            await _sprintService.UpdateAsync(item, User.GetOnlineUser());
        }

        return RedirectToAction(nameof(Index));
    }
}