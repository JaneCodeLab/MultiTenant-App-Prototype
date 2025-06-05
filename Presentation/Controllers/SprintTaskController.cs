
using ApplicationCore.DomainModel;
using ApplicationCore.DomainService;
using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Controllers;

[Authorize]
public class SprintTaskController : BaseController
{
    private readonly ILogger<SprintTaskController> _logger;
    private readonly ISprintTaskService _sprintTaskService;
    private readonly ISprintService _sprintService;
    private readonly IProjectService _projectService;
    private readonly ITaskAssigneeService _taskAssigneeService;
    private readonly IDepartmentMemberService _departmentMemberService;
    private readonly ISysCustomUserService _sysCustomUserService;

    public SprintTaskController(ILogger<SprintTaskController> logger,
                             IDataProtectionProvider dataProtectionProvider,
                             IProjectService projectService,
                             ISprintService sprintService,
                             ITaskAssigneeService taskAssigneeService,
                             IDepartmentMemberService departmentMemberService,
                             ISysCustomUserService sysCustomUserService,
                             ISprintTaskService sprintTaskService)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _sprintTaskService = sprintTaskService;
        _projectService = projectService;
        _sprintService = sprintService;
        _taskAssigneeService = taskAssigneeService;
        _departmentMemberService = departmentMemberService;
        _sysCustomUserService = sysCustomUserService;
    }
    [HttpGet]
    public async Task<IActionResult> Create(int sprintId, int departmentId, Guid? issueId = null)
    {
        var sprint = await _sprintService.GetAsync(sprintId);
        var langEnum = User.GetLoggedInUserLanguageEnum();

        if (sprint == null)
        {
            return RedirectToAction(nameof(ErrorsController.Message),
                                    nameof(ErrorsController).Replace("Controller", ""),
                                    new
                                    {
                                        menuTitle = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Sprints.ToInt()),
                                        message = SysExpressionHelper.Get(langEnum, ExpressionTypes.Sprint, SprintExpression.ThereIsNoSprint.ToInt()),
                                        returnUrl = $"/{nameof(SprintController).Replace("Controller", "")}"
                                    });
        }

        if (sprint.Backlog)
        {
            ViewBag.Menu = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Backlog.ToInt());
            ViewBag.CallBackController = nameof(BacklogController).Replace("Controller", "");
            ViewBag.PageTitle = SysExpressionHelper.Get(langEnum, ExpressionTypes.Backlog, BacklogExpression.Backlogs.ToInt());
        }
        else if (sprint.Current)
        {
            ViewBag.Menu = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.CurrentSprint.ToInt());
            ViewBag.CallBackController = nameof(CurrentSprintController).Replace("Controller", "");
            ViewBag.PageTitle = SysExpressionHelper.Get(langEnum, ExpressionTypes.CurrentSprint, CurrentSprintExpression.CurrentSprint.ToInt());
        }
        else
        {
            ViewBag.Menu = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Sprints.ToInt());
            ViewBag.CallBackController = nameof(SprintController).Replace("Controller", "");
            ViewBag.PageTitle = SysExpressionHelper.Get(langEnum, ExpressionTypes.Sprint, SprintExpression.Sprints.ToInt());
        }
        var memberList = await _departmentMemberService.GetDepartmentMembers(departmentId);
        ViewData["MemberList"] = new SelectList(memberList, nameof(DepartmentMember.UserId), nameof(DepartmentMember.FullName));
        ViewData["ProjectList"] = new SelectList(await _projectService.GetDepartmentProjectsAsync(departmentId), nameof(Department.Id), nameof(Department.Title));

        var model = new SprintTask { SprintId = sprintId, IssueId = issueId, Deadline = sprint.EndDate };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SprintTask model)
    {
        model.Id = Guid.NewGuid();
        var sprint = await _sprintService.GetAsync(model.SprintId);
        if (ModelState.IsValid)
        {
            var result = await _sprintTaskService.CreateAsync(model, User.GetOnlineUser());
            if (result.Type == ServiceResultType.Succeed)
            {
                if (sprint.Backlog)
                    return RedirectToAction(nameof(Index), nameof(BacklogController).Replace("Controller", ""));
                else if (sprint.Current)
                    return RedirectToAction(nameof(Index), nameof(CurrentSprintController).Replace("Controller", ""));
                else
                    return RedirectToAction(nameof(Index), nameof(SprintController).Replace("Controller", ""));
            }
            else
                ModelState.AddModelError(nameof(model.Code), result.Message);
        }
        var langEnum = User.GetLoggedInUserLanguageEnum();
        if (sprint.Backlog)
        {
            ViewBag.Menu = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Backlog.ToInt());
            ViewBag.CallBackController = nameof(BacklogController).Replace("Controller", "");
            ViewBag.PageTitle = SysExpressionHelper.Get(langEnum, ExpressionTypes.Backlog, BacklogExpression.Backlogs.ToInt());
        }
        else if (sprint.Current)
        {
            ViewBag.Menu = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.CurrentSprint.ToInt());
            ViewBag.CallBackController = nameof(CurrentSprintController).Replace("Controller", "");
            ViewBag.PageTitle = SysExpressionHelper.Get(langEnum, ExpressionTypes.CurrentSprint, CurrentSprintExpression.CurrentSprint.ToInt());
        }
        else
        {
            ViewBag.Menu = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Sprints.ToInt());
            ViewBag.CallBackController = nameof(SprintController).Replace("Controller", "");
            ViewBag.PageTitle = SysExpressionHelper.Get(langEnum, ExpressionTypes.Sprint, SprintExpression.Sprints.ToInt());
        }

        var memberList = await _departmentMemberService.GetDepartmentMembers(sprint.DepartmentId);
        ViewData["MemberList"] = new SelectList(memberList, nameof(DepartmentMember.UserId), nameof(DepartmentMember.FullName));
        ViewData["ProjectList"] = new SelectList(await _projectService.GetDepartmentProjectsAsync(sprint.DepartmentId), nameof(Department.Id), nameof(Department.Title), model.ProjectId);
        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, int departmentId)
    {
        var model = await _sprintTaskService.GetAsync(id);
        model.Description = StringCompressor.Decompress(model.Description);

        var sprint = await _sprintService.GetAsync(model.SprintId);
        if (sprint != null)
        {
            var langEnum = User.GetLoggedInUserLanguageEnum();
            if (sprint.Backlog)
            {
                ViewBag.Menu = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Backlog.ToInt());
                ViewBag.CallBackController = nameof(BacklogController).Replace("Controller", "");
                ViewBag.PageTitle = SysExpressionHelper.Get(langEnum, ExpressionTypes.Backlog, BacklogExpression.Backlogs.ToInt());
            }
            else if (sprint.Current)
            {
                ViewBag.Menu = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.CurrentSprint.ToInt());
                ViewBag.CallBackController = nameof(CurrentSprintController).Replace("Controller", "");
                ViewBag.PageTitle = SysExpressionHelper.Get(langEnum, ExpressionTypes.CurrentSprint, CurrentSprintExpression.CurrentSprint.ToInt());
            }
            else
            {
                ViewBag.Menu = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Sprints.ToInt());
                ViewBag.CallBackController = nameof(SprintController).Replace("Controller", "");
                ViewBag.PageTitle = SysExpressionHelper.Get(langEnum, ExpressionTypes.Sprint, SprintExpression.Sprints.ToInt());
            }
        }
        var memberList = await _departmentMemberService.GetDepartmentMembers(departmentId);
        ViewData["MemberList"] = new SelectList(memberList, nameof(DepartmentMember.UserId), nameof(DepartmentMember.FullName));
        ViewData["ProjectList"] = new SelectList(await _projectService.GetDepartmentProjectsAsync(sprint.DepartmentId), nameof(Department.Id), nameof(Department.Title), model.ProjectId);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(SprintTask model)
    {
        var sprint = await _sprintService.GetAsync(model.SprintId);
        if (ModelState.IsValid)
        {
            var result = await _sprintTaskService.CustomUpdateAsync(model, User.GetOnlineUser());
            if (result.Type == ServiceResultType.Succeed)
            {
                if (sprint.Backlog)
                    return RedirectToAction(nameof(Index), nameof(BacklogController).Replace("Controller", ""));
                else if (sprint.Current)
                    return RedirectToAction(nameof(Index), nameof(CurrentSprintController).Replace("Controller", ""));
                else
                    return RedirectToAction(nameof(Index), nameof(SprintController).Replace("Controller", ""));
            }
            else
                ModelState.AddModelError(nameof(model.Code), result.Message);
        }

        var langEnum = User.GetLoggedInUserLanguageEnum();
        if (sprint.Backlog)
        {
            ViewBag.Menu = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Backlog.ToInt());
            ViewBag.CallBackController = nameof(BacklogController).Replace("Controller", "");
            ViewBag.PageTitle = SysExpressionHelper.Get(langEnum, ExpressionTypes.Backlog, BacklogExpression.Backlogs.ToInt());
        }
        else if (sprint.Current)
        {
            ViewBag.Menu = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.CurrentSprint.ToInt());
            ViewBag.CallBackController = nameof(CurrentSprintController).Replace("Controller", "");
            ViewBag.PageTitle = SysExpressionHelper.Get(langEnum, ExpressionTypes.CurrentSprint, CurrentSprintExpression.CurrentSprint.ToInt());
        }
        else
        {
            ViewBag.Menu = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Sprints.ToInt());
            ViewBag.CallBackController = nameof(SprintController).Replace("Controller", "");
            ViewBag.PageTitle = SysExpressionHelper.Get(langEnum, ExpressionTypes.Sprint, SprintExpression.Sprints.ToInt());
        }

        var memberList = await _departmentMemberService.GetDepartmentMembers(sprint.DepartmentId);
        ViewData["MemberList"] = new SelectList(memberList, nameof(DepartmentMember.UserId), nameof(DepartmentMember.FullName));
        ViewData["ProjectList"] = new SelectList(await _projectService.GetDepartmentProjectsAsync(sprint.DepartmentId), nameof(Department.Id), nameof(Department.Title), model.ProjectId);
        return View(model);
    }

    public async Task<JsonResult> Delete(Guid id) => new JsonResult(await _sprintTaskService.PermanentDeleteAsync(User.GetOnlineUser(), id));

    public async Task<IActionResult> AddAssignee(Guid sprintTaskId, string assigneeId)
    {
        var sprinTask = await _sprintTaskService.GetAsync(sprintTaskId);
        if (!sprinTask.Assignees.Any(c => c.UserId == assigneeId))
        {
            var user = await _sysCustomUserService.FindAsync(assigneeId);
            var taskOwner = new TaskAssignee
            {
                SprintTaskId = sprintTaskId,
                UserId = assigneeId,
                ProfileImage = user.ProfileImage ?? (user.Gender == Gender.Female ? "default_female.png" : "default_male.png"),
                FullName = user.GetFullName(),
            };
            await _taskAssigneeService.CreateAsync(taskOwner, User.GetOnlineUser());
        }
        return RedirectToAction(nameof(Edit), new { departmentId = sprinTask.DepartmentId, id = sprinTask.Id });
    }

    public async Task<IActionResult> RemoveAssignee(Guid id)
    {
        var taskOwner = await _taskAssigneeService.GetAsync(id);
        var sprintItemId = taskOwner.SprintTaskId;
        await _taskAssigneeService.PermanentDeleteAsync(User.GetOnlineUser(), id);
        return RedirectToAction(nameof(Edit), new { id = sprintItemId });
    }

    public async Task<IActionResult> ChangeState(Guid id, bool activate)
    {
        await _sprintTaskService.ChangeActivateAsync(id, !activate, User.GetOnlineUser());
        return RedirectToAction(nameof(Index));
    }
}