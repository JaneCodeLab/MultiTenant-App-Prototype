
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Authorize]
    public class ChooseTenantController : BaseController
    {
        private readonly ILogger<ChooseTenantController> _logger;
        private readonly UserManager<SysCustomUser> _userManager;
        private readonly SignInManager<SysCustomUser> _signInManager;
        private readonly ISysTenantUserService _tenantUserService;


        public ChooseTenantController(ILogger<ChooseTenantController> logger,
                                      UserManager<SysCustomUser> userManager,
                                      SignInManager<SysCustomUser> signInManager,
                                      IDataProtectionProvider dataProtectionProvider,
                                      ISysTenantUserService tenantUserService)
                            : base(dataProtectionProvider)
        {
            _logger = logger;
            _tenantUserService = tenantUserService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            var tenants = await _tenantUserService.GetUsersTenants(User.GetLoggedInUserId());
            if (!tenants.Any())
            {
                return RedirectToAction(nameof(NotFound));
            }
            if (User.IsInRole(Roles.Admin) || tenants.Distinct().Count() == 1)
            {
                var user = await _userManager.FindByNameAsync(User.GetLoggedInUserName());
                await _userManager.AddClaimAsync(user, new Claim(CustomClaimType.TenantId, tenants.FirstOrDefault().TenantId.ToString()));
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Index", "SelectBranch", new { returnUrl });
            }
            else
            {
                ViewBag.UsersTenants = tenants;
                return View();
            }

        }

        [HttpPost]
        public async Task<IActionResult> Index(int tenantId, string returnUrl = null)
        {
            var user = await _userManager.FindByNameAsync(User.GetLoggedInUserName());

            var tenantIdClaim = User.Claims.Where(x => x.Type == CustomClaimType.TenantId).FirstOrDefault();
            if (tenantIdClaim != null)
                await _userManager.RemoveClaimAsync(user, tenantIdClaim);

            var SquadIdClaim = User.Claims.Where(x => x.Type == CustomClaimType.Squad).FirstOrDefault();
            if (SquadIdClaim != null)
                await _userManager.RemoveClaimAsync(user, SquadIdClaim);

            var SquadTitleClaim = User.Claims.Where(x => x.Type == CustomClaimType.SquadTitle).FirstOrDefault();
            if (SquadTitleClaim != null)
                await _userManager.RemoveClaimAsync(user, SquadTitleClaim);

            var SquadCountClaim = User.Claims.Where(x => x.Type == CustomClaimType.SquadCounts).FirstOrDefault();
            if (SquadCountClaim != null)
                await _userManager.RemoveClaimAsync(user, SquadCountClaim);

            await _userManager.AddClaimAsync(user, new Claim(CustomClaimType.TenantId, tenantId.ToString()));
            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction("Index", "SelectBranch", new { returnUrl });
        }

        public async Task<IActionResult> NotFound(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}