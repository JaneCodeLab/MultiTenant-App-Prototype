
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize]
public class CurrentSprintController : BaseController
{
    private readonly ILogger<CurrentSprintController> _logger;
    private readonly ISprintService _sprintService;
    private readonly ISprintPlanService _sprintPlanService;
    private readonly ISprintTaskService _sprintTaskService;
    private readonly IDepartmentService _departmentService;

    public CurrentSprintController(ILogger<CurrentSprintController> logger,
                             IDataProtectionProvider dataProtectionProvider,
                             IDepartmentService departmentService,
                             ISprintPlanService sprintPlanService,
                             ISprintTaskService sprintTaskService,
                             ISprintService sprintService)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _sprintService = sprintService;
        _sprintPlanService = sprintPlanService;
        _departmentService = departmentService;
        _sprintTaskService = sprintTaskService;
    }

    public async Task<IActionResult> Index()
    {
        var departmentId = (User.GetLoggedInUserSquad()).ToInt();
        ViewBag.DepartmentId = departmentId;
        var currentSprintId = await _sprintService.GetCurrentSprintIdAsync(departmentId);
        ViewBag.CurrentSprintId = currentSprintId;
        var model = new ViewListModel<SprintPlanMinimalListItem, BaseFilter>
        {
            Records = await _sprintPlanService.GetSprintPlanItems(currentSprintId ?? 0),
            Filter = new()
        };
        return View(model);
    }

    public async Task<JsonResult> AddToBacklog(Guid id) => new JsonResult(await _sprintTaskService.AddToBacklog(id, User.GetOnlineUser()));
}