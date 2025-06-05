
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Controllers;

[Authorize]
public class SprintReviewReportController : BaseController
{
    private readonly ILogger<SprintReviewController> _logger;
    private readonly IActivityService _activityService;
    private readonly ISprintTaskService _sprintTaskService;
    private readonly IDepartmentMemberService _departmentMemberService;
    private readonly ISprintService _sprintService;
    private readonly IDepartmentService _departmentService;

    public SprintReviewReportController(ILogger<SprintReviewController> logger,
                             IDataProtectionProvider dataProtectionProvider,
                             IDepartmentService departmentService,
                             ISprintService sprintService,
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
        _sprintService = sprintService;
    }

    public async Task<IActionResult> Index(string userId, int sprintId = 0)
    {
        var departmentId = (User.GetLoggedInUserSquad()).ToInt();
        var sprints = (await _sprintService.GetDepartmentSprintsAsync(departmentId)).OrderBy(o => o.Title);
        sprintId = (sprintId == 0 ? sprints.Where(c => c.Current)?.FirstOrDefault()?.Id : sprints.FirstOrDefault(c => c.Id == sprintId)?.Id) ?? 0;

        //ToDo: After Implementing Capacity This List Should be fetched from Capacity
        var members = await _departmentMemberService.GetDepartmentMembers(departmentId);

        if (userId.IsNullOrEmpty() || !members.Any(c => c.UserId == userId))
            userId = members.FirstOrDefault()?.UserId;

        ViewData["Members"] = members;
        ViewBag.CurrentSprintId = sprintId;
        ViewBag.DepartmentId = departmentId;
        ViewBag.CurrentMember = userId;
        ViewBag.CurrentMemberFullName = members.FirstOrDefault(c => c.UserId == userId)?.FullName;
        ViewData["SprintList"] = new SelectList(sprints, nameof(Sprint.Id), nameof(Sprint.Title), sprintId);

        return View(await _sprintTaskService.GetUsersSprintsTasksAsync(userId, sprintId));
    }
}