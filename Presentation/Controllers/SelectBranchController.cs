
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers;

[Authorize]
public class SelectBranchController : BaseController
{
    private readonly ILogger<SelectBranchController> _logger;
    private readonly UserManager<SysCustomUser> _userManager;
    private readonly SignInManager<SysCustomUser> _signInManager;
    private readonly IDepartmentService _departmentService;

    public SelectBranchController(ILogger<SelectBranchController> logger,
                                  UserManager<SysCustomUser> userManager,
                                  SignInManager<SysCustomUser> signInManager,
                                  IDepartmentService departmentService,
                                  IDataProtectionProvider dataProtectionProvider)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _departmentService = departmentService;
    }

    public async Task<IActionResult> Index(string returnUrl = null)
    {
        var user = await _userManager.FindByNameAsync(User.GetLoggedInUserName());

        if (User.IsInRole(Roles.DevPersonnel) || User.IsInRole(Roles.LimitedAccess)
                                                || User.IsInRole(Roles.CompanyAdmin)
                                                || User.IsInRole(Roles.Admin)
                                                || User.IsInRole(Roles.SalesPersonnel))
        {
            var departments = await _departmentService.GetDepartmentForSelect();
            if (departments.Any())
            {
                await _userManager.AddClaimAsync(user, new Claim(CustomClaimType.Squad, departments.FirstOrDefault()?.Id.ToString()));
                await _userManager.AddClaimAsync(user, new Claim(CustomClaimType.SquadTitle, departments.FirstOrDefault()?.Title));
                await _userManager.AddClaimAsync(user, new Claim(CustomClaimType.SquadCounts, departments.Count.ToString()));
            }
            else
                return Redirect("/Identity/Account/Logout");
        }
        else
        {
            var departments = await _departmentService.GetUsersDepartmentForSelect(c => c.Title, user.Id);
            if (departments.Any())
            {
                await _userManager.AddClaimAsync(user, new Claim(CustomClaimType.Squad, departments.FirstOrDefault()?.Id.ToString()));
                await _userManager.AddClaimAsync(user, new Claim(CustomClaimType.SquadTitle, departments.FirstOrDefault()?.Title));
                await _userManager.AddClaimAsync(user, new Claim(CustomClaimType.SquadCounts, departments.Count.ToString()));
            }
            else
                return Redirect("/Identity/Account/Logout");
        }

        await _signInManager.RefreshSignInAsync(user);
        return LocalRedirect(returnUrl);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeSquad(int squadId)
    {
        var user = await _userManager.FindByNameAsync(User.GetLoggedInUserName());
        var department = await _departmentService.GetAsync(squadId);

        var SquadIdClaim = User.Claims.Where(x => x.Type == CustomClaimType.Squad).FirstOrDefault();
        if (SquadIdClaim != null)
            await _userManager.RemoveClaimAsync(user, SquadIdClaim);

        var SquadTitleClaim = User.Claims.Where(x => x.Type == CustomClaimType.SquadTitle).FirstOrDefault();
        if (SquadTitleClaim != null)
            await _userManager.RemoveClaimAsync(user, SquadTitleClaim);

        var SquadCountClaim = User.Claims.Where(x => x.Type == CustomClaimType.SquadCounts).FirstOrDefault();
        if (SquadCountClaim != null)
            await _userManager.RemoveClaimAsync(user, SquadCountClaim);

        if (User.IsInRole(Roles.DevPersonnel) || User.IsInRole(Roles.LimitedAccess)
                                               || User.IsInRole(Roles.CompanyAdmin)
                                               || User.IsInRole(Roles.Admin)
                                               || User.IsInRole(Roles.SalesPersonnel))
        {
            var departments = await _departmentService.GetDepartmentForSelect();
            await _userManager.AddClaimAsync(user, new Claim(CustomClaimType.SquadCounts, departments.Count.ToString()));
        }
        else
        {
            var departments = await _departmentService.GetUsersDepartmentForSelect(c => c.Title, user.Id);
            await _userManager.AddClaimAsync(user, new Claim(CustomClaimType.SquadCounts, departments.Count.ToString()));
        }

        await _userManager.AddClaimAsync(user, new Claim(CustomClaimType.Squad, department.Id.ToString()));
        await _userManager.AddClaimAsync(user, new Claim(CustomClaimType.SquadTitle, department.Title));
        await _signInManager.RefreshSignInAsync(user);
        return Ok();
    }
}