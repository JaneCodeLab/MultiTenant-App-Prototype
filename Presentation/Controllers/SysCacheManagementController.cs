
using ApplicationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize(Roles = Roles.DevPersonnel + "," + Roles.Admin)]
public class SysCacheManagementController : BaseController
{
    private readonly ILogger<SysCacheManagementController> _logger;
    private readonly ISysTenantService _tenantService;
    private readonly ISysExpressionService _expressionService;
    private readonly ISysParameterService _parameterService;

    public SysCacheManagementController(ILogger<SysCacheManagementController> logger,
                                    IDataProtectionProvider dataProtectionProvider,
                                    ISysExpressionService expressionService,
                                    ISysParameterService parameterService,
                                    ISysTenantService tenantService)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _tenantService = tenantService;
        _expressionService = expressionService;
        _parameterService = parameterService;

    }

    public async Task<IActionResult> ClearCache()
    {
        _expressionService.FetchAllExpressions();
        _parameterService.FetchAllParameters();
        _tenantService.FetchHelperTenants();

        return View();

    }
}