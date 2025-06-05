
using ApplicationCore.DomainModel;
using ApplicationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Controllers;

[Authorize(Roles = Roles.Admin)]
public class SysRoleUserController : BaseController
{
    private readonly UserManager<SysCustomUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ISysCustomUserService _userService;
    public SysRoleUserController(IDataProtectionProvider dataProtectionProvider,
                              UserManager<SysCustomUser> userManager,
                              ISysCustomUserService userService,
                              RoleManager<IdentityRole> roleManager)
                        : base(dataProtectionProvider)
    {
        _userService = userService;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["UserList"] = new SelectList(await _userService.GetEmployeeUserListAsync(), nameof(SysCustomUser.Id), nameof(SysCustomUser.Email));

        List<Tuple<string, string, IList<SysCustomUser>>> result = new();
        foreach (var role in _roleManager.Roles.ToList())
        {
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            result.Add(new(role.Name, role.Id, users.OrderBy(o => o.FirstName).ThenBy(o => o.LastName).ToList()));
        }
        return View(result);
    }

    [HttpPost]
    public async Task<IActionResult> Assign(string role, List<string> userIds)
    {
        var users = await _userService.GetListAsync(userIds);

        List<IdentityResult> result = new List<IdentityResult>();
        foreach (var user in users)
            result.Add(await _userManager.AddToRoleAsync(user, role));

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Remove(string role, string userId)
    {
        await _userManager.RemoveFromRoleAsync(await _userService.FindAsync(userId), role);
        return RedirectToAction(nameof(Index));
    }
}