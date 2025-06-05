using ApplicationService;
using Infrastructure.Helpers;
using Infrastructure.SqlServerAdapter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISysTenantUserService _tenantUserService;
        private readonly ISysTenantService _tenantService;
        private readonly ISysExpressionService _sysExpressionService;
        private readonly IHostEnvironment _environment;
        private readonly IDepartmentService _departmentService;
        private readonly ISysReleaseNoteService _sysReleaseNoteService;

        public HomeController(ILogger<HomeController> logger,
                                IDataProtectionProvider dataProtectionProvider,
                                ISysExpressionService sysExpressionService,
                                IHostEnvironment environment,
                                IDepartmentService departmentService,
                                ISysReleaseNoteService sysReleaseNoteService,
                                ISysTenantUserService tenantUserService,
                                ISysTenantService tenantService)
                            : base(dataProtectionProvider)
        {
            _logger = logger;
            _tenantService = tenantService;
            _sysExpressionService = sysExpressionService;
            _environment = environment;
            _sysReleaseNoteService = sysReleaseNoteService;
            _departmentService = departmentService;
            _tenantUserService = tenantUserService;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.IsInRole(Roles.Admin))
                return View();


            var tenantInfoList = new List<VmTenant>();
            foreach (var tenant in TenantHelper.Tenants)
            {
                var tenantInfo = new VmTenant()
                {
                    TenantId = tenant.TenantId,
                    Title = tenant.Title,
                    Logo = tenant.Logo,
                };
                //ToDo: Just Get the Count not all data
                tenantInfo.UserCount = (await _tenantUserService.GetTenantsUsers(tenant.TenantId)).Count;
                var tenantProvider = new TenantProvider(tenant.TenantId);
                try
                {
                    using (var context = new TenantDbContext(tenantProvider))
                    {
                        tenantInfo.Active = true;

                        tenantInfo.DepartmentCount = context.Departments.Count();
                        tenantInfo.CustomerCount = context.Customers.Count();
                        tenantInfo.ProjectCount = context.Projects.Count();
                        tenantInfo.SprintCount = context.Sprints.Count();
                        tenantInfo.SprintTaskCount = context.SprintTasks.Count();
                        tenantInfo.LogCount = context.Logs.Count();
                        tenantInfo.ApiLogCount = context.ApiLogs.Count();
                    }
                }
                catch
                {
                    tenantInfo.Active = false;
                }

                tenantInfoList.Add(tenantInfo);
            }

            ViewBag.TenantList = tenantInfoList;
            return View();
        }

        public async Task<IActionResult> TenantList()
        {
            return View(TenantHelper.Tenants);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> VersionHistory()
        {
            return View(await _sysReleaseNoteService.GetAllAsync(new BaseFilter(), User.GetLoggedInUserTimezoneId()));
        }

        public async Task<JsonResult> GetSquads()
        {
            if (User.IsInRole(Roles.Admin) || User.IsInRole(Roles.CompanyAdmin) || User.IsInRole(Roles.DevPersonnel) || User.IsInRole(Roles.SalesPersonnel))
                return new JsonResult(await _departmentService.GetDepartmentForSelect());
            else
                return new JsonResult(await _departmentService.GetUsersDepartmentForSelect(o => o.Title, User.GetLoggedInUserId()));
        }
    }
}