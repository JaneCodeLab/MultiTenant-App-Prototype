
using ApplicationCore.DomainModel;
using ApplicationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize(Roles = Roles.DevPersonnel + "," + Roles.Admin)]
public class SysParameterController : BaseController
{
    private readonly ILogger<SysParameterController> _logger;
    private readonly ISysParameterService _sysParameterService;

    public SysParameterController(ILogger<SysParameterController> logger,
                                   IDataProtectionProvider dataProtectionProvider,
                                   ISysParameterService sysParameterService) : base(dataProtectionProvider)
    {
        _logger = logger;
        _sysParameterService = sysParameterService;
    }

    public async Task<IActionResult> Index()
    {
        foreach (Language lang in (Language[])Enum.GetValues(typeof(Language)))
            await _sysParameterService.CheckMappingsValidity(lang, true);

        return View(await _sysParameterService.GetAllAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        return View(await _sysParameterService.GetAsync(id));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(DtoSysParameter viewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _sysParameterService.UpdateEquivalentAsync(viewModel.Id, viewModel.Equivalent, User.GetOnlineUser());
            if (result.Type == ServiceResultType.Succeed)
            {
                SysExpressionHelper.Items.Clear();
                _sysParameterService.FetchAllParameters();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(nameof(viewModel.Equivalent), result.Message);
                return View(viewModel);
            }
        }

        return View(viewModel);
    }
}
