
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Controllers;

//[Authorize(Roles = $"{Roles.DevPersonnel},{Roles.Admin},{Roles.CompanyAdmin}")]
[Authorize]
public class DepartmentMemberController : BaseController
{
    private readonly ILogger<DepartmentMemberController> _logger;
    private readonly IDepartmentMemberService _departmentMemberService;
    private readonly IDepartmentService _departmentService;
    private readonly ISysCustomUserService _sysCustomUserService;
    private readonly IDepartmentRoleService _departmentRoleService;

    public DepartmentMemberController(ILogger<DepartmentMemberController> logger,
                             IDataProtectionProvider dataProtectionProvider,
                             IDepartmentService departmentService,
                             IDepartmentRoleService departmentRoleService,
                             ISysCustomUserService sysCustomUserService,
                             IDepartmentMemberService departmentMemberService)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _departmentService = departmentService;
        _departmentMemberService = departmentMemberService;
        _departmentRoleService = departmentRoleService;
        _sysCustomUserService = sysCustomUserService;
    }

    public async Task<IActionResult> Index()
    {
        var departmentId = (User.GetLoggedInUserSquad()).ToInt();
        var memberList = await _departmentMemberService.GetDepartmentMembers(departmentId);
        string members = "";
        foreach (var item in memberList)
        {
            members +=
                "{ id: '" + item.Id + "', " +
                  "name: '" + item.FullName + "', " +
                  "role: '" + item.DepartmentRole?.Title + "', " +
                  $"description: '/img/user/general/{item.ProfileImage}', " +
                  "parent: " + (item.SupervisorMemberId == null ? "'0', " : "'" + item.SupervisorMemberId + "',") +
                  $"btn1: '<a title=\"Add Subordinate\" class=\"description-btn\" onclick=\"event.preventDefault();openModal(``,``,`{item.Id}`,``)\" >Sub</a>'," +
                  $"btn2: '<a title=\"Edit\" class=\"description-btn\" onclick=\"event.preventDefault();openModal(`{item.Id}`,`{item.UserId}`,`{item.SupervisorMemberId}`,`{item.DepartmentRoleId}`)\">Edit</a>'," +
                  $"btn3: '<a title=\"Delete\" class=\"description-btn\" onclick=\"event.preventDefault();deleteConfirm(`{item.Id}`)\">Delete</a>'," +
                " },";
        }

        var tenantPersonnelList = await _sysCustomUserService.GetTenantEmployeeUserListAsync(User.GetCurrentTenantId());
        ViewData["PersonnelList"] = new SelectList(tenantPersonnelList.Select(s=> new { Id = s.Id, Fullname = $"{s.FirstName} {s.LastName}"}), 
                                                    nameof(SysCustomUser.Id),
                                                    "Fullname");

        ViewData["MemberList"] = new SelectList(memberList, nameof(DepartmentMember.Id), nameof(DepartmentMember.FullName));
        ViewData["DepartmentRoleList"] = new SelectList(await _departmentRoleService.GetDepartmentRoles(departmentId), nameof(DepartmentRole.Id), nameof(DepartmentRole.Title));

        ViewBag.Members = members;
        ViewBag.DepartmentId = departmentId;
        ViewBag.DepartmentName = User.GetLoggedInUserSquadTitle();

        return View(memberList);
    }

    [HttpPost]
    public async Task<JsonResult> Save(DepartmentMember model)
    {
        ServiceResult result;
        if (model.Id != Guid.Empty)
            result = await _departmentMemberService.CustomUpdateAsync(model, User.GetOnlineUser());
        else
        {
            model.Id = Guid.NewGuid();
            result = await _departmentMemberService.CreateAsync(model, User.GetOnlineUser());
        }

        return new JsonResult(result);
    }

    [HttpPost]
    public async Task<JsonResult> Delete(Guid id) => new JsonResult(await _departmentMemberService.PermanentDeleteAsync(User.GetOnlineUser(), id));

    public async Task<IActionResult> ChangeState(Guid id, bool activate)
    {
        await _departmentMemberService.ChangeActivateAsync(id, !activate, User.GetOnlineUser());
        return RedirectToAction(nameof(Index));
    }
}