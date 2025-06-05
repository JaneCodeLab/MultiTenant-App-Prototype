
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize]
public class SprintReviewController : BaseController
{
    private readonly ILogger<SprintReviewController> _logger;
    private readonly IActivityService _activityService;
    private readonly ISprintTaskService _sprintTaskService;
    private readonly IDepartmentMemberService _departmentMemberService;
    private readonly IDepartmentService _departmentService;

    public SprintReviewController(ILogger<SprintReviewController> logger,
                             IDataProtectionProvider dataProtectionProvider,
                             IDepartmentService departmentService,
                             IDepartmentMemberService departmentMemberService,
                             IActivityService activityService,
                             ISprintTaskService sprintTaskService)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _activityService = activityService;
        _sprintTaskService = sprintTaskService;
        _departmentMemberService = departmentMemberService;
        _departmentService = departmentService;
    }

    public async Task<IActionResult> Index(string userId)
    {
        var departmentId = (User.GetLoggedInUserSquad()).ToInt();
        var members = await _departmentMemberService.GetDepartmentMembers(departmentId);

        if (userId.IsNullOrEmpty() || !members.Any(c => c.UserId == userId))
            userId = members.FirstOrDefault()?.UserId;

        ViewData["Members"] = members;
        ViewBag.DepartmentId = departmentId;
        ViewBag.CurrentMember = userId;
        ViewBag.CurrentMemberFullName = members.FirstOrDefault(c => c.UserId == userId)?.FullName;

        return View(await _sprintTaskService.GetUsersCurrentTasksAsync(userId, departmentId));
    }

    [HttpPost]
    public async Task<JsonResult> Save(SprintTask model)
    {
        ServiceResult result;
        model.Title = model.Title?.Trim() ?? string.Empty;
        result = await _sprintTaskService.UpdateTimeAsync(model, User.GetOnlineUser());

        return new JsonResult(result);
    }

    public async Task<JsonResult> GetDescription(Guid id)
    {
        var task = await _sprintTaskService.GetAsync(id);

        return new JsonResult(StringCompressor.Decompress(task.Description));
    }

    public async Task<IActionResult> MakeTodo(Guid id, string userId)
    {
        await _sprintTaskService.ChangeProressStatus(id, ProgressStatus.ToDo, User.GetOnlineUser());
        return RedirectToAction(nameof(Index), new { userId });
    }

    public async Task<IActionResult> MakeInProgress(Guid id, string userId)
    {
        await _sprintTaskService.ChangeProressStatus(id, ProgressStatus.InProgress, User.GetOnlineUser());
        return RedirectToAction(nameof(Index), new { userId });
    }

    public async Task<IActionResult> MakeCancel(Guid id, string userId)
    {
        await _sprintTaskService.ChangeProressStatus(id, ProgressStatus.Cancel, User.GetOnlineUser());
        return RedirectToAction(nameof(Index), new { userId });
    }

    public async Task<IActionResult> MakeDone(Guid id, string userId)
    {
        await _sprintTaskService.ChangeProressStatus(id, ProgressStatus.Done, User.GetOnlineUser());
        return RedirectToAction(nameof(Index), new { userId });
    }
}