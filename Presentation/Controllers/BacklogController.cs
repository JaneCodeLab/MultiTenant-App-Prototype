
using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize]
public class BacklogController : BaseController
{
    private readonly ILogger<BacklogController> _logger;
    private readonly IBacklogService _backlogService;
    private readonly ISprintService _sprintService;
    private readonly ISprintTaskService _sprintTaskService;
    private readonly IDepartmentService _departmentService;

    public BacklogController(ILogger<BacklogController> logger,
                             IDataProtectionProvider dataProtectionProvider,
                             IDepartmentService departmentService,
                             ISprintService sprintService,
                             ISprintTaskService sprintTaskService,
                             IBacklogService backlogService)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _backlogService = backlogService;
        _sprintService = sprintService;
        _departmentService = departmentService;
        _sprintTaskService = sprintTaskService;
    }

    public async Task<IActionResult> Index()
    {
        var departmentId = User.GetLoggedInUserSquad().ToInt();

        ViewBag.DepartmentId = departmentId;
        ViewBag.BacklogId = await _sprintService.GetBacklogIdAsync(departmentId);
        return View(await _backlogService.GetBacklogItems(departmentId));
    }

    public async Task<JsonResult> AddToCurrentSprint(Guid id) => new JsonResult(await _sprintTaskService.AddToCurrentSprint(id, User.GetOnlineUser()));
}