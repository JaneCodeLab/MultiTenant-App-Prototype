
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize]
public class MyTasksController : BaseController
{
    private readonly ILogger<MyTasksController> _logger;
    private readonly IActivityService _activityService;
    private readonly ISprintTaskService _sprintTaskService;

    public MyTasksController(ILogger<MyTasksController> logger,
                             IDataProtectionProvider dataProtectionProvider,
                             IActivityService activityService,
                             ISprintTaskService sprintTaskService)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _activityService = activityService;
        _sprintTaskService = sprintTaskService;
    }

    public async Task<IActionResult> Index()
    {
        var departmentId = (User.GetLoggedInUserSquad()).ToInt();
        return View(await _sprintTaskService.GetUsersCurrentTasksAsync(User.GetLoggedInUserId(), departmentId));
    }

    [HttpPost]
    public async Task<JsonResult> Save(SprintTask model)
    {
        ServiceResult result;
        model.Title = model.Title?.Trim() ?? string.Empty;
        result = await _sprintTaskService.UpdateTimeAsync(model, User.GetOnlineUser());

        return new JsonResult(result);
    }

    public async Task<IActionResult> MakeTodo(Guid id)
    {
        await _sprintTaskService.ChangeProressStatus(id, ProgressStatus.ToDo, User.GetOnlineUser());
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> MakeInProgress(Guid id)
    {
        await _sprintTaskService.ChangeProressStatus(id, ProgressStatus.InProgress, User.GetOnlineUser());
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> MakeCancel(Guid id)
    {
        await _sprintTaskService.ChangeProressStatus(id, ProgressStatus.Cancel, User.GetOnlineUser());
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> MakeDone(Guid id)
    {
        await _sprintTaskService.ChangeProressStatus(id, ProgressStatus.Done, User.GetOnlineUser());
        return RedirectToAction(nameof(Index));
    }
}