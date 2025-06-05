
using ApplicationCore.DomainModel;
using ApplicationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize(Roles = Roles.DevPersonnel + "," + Roles.Admin)]
public class SysExpressionController : BaseController
{
    private readonly ILogger<SysExpressionController> _logger;
    private readonly ISysExpressionService _sysExpressionService;

    public SysExpressionController(ILogger<SysExpressionController> logger,
                                   IDataProtectionProvider dataProtectionProvider,
                                   ISysExpressionService sysExpressionService) : base(dataProtectionProvider)
    {
        _logger = logger;
        _sysExpressionService = sysExpressionService;
    }

    public async Task<IActionResult> Index()
    {
        foreach (Language lang in (Language[])Enum.GetValues(typeof(Language)))
            await _sysExpressionService.CheckMappingsValidity(lang, true);

        return View(await _sysExpressionService.GetAllAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        return View(await _sysExpressionService.GetAsync(id));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(DtoSysExpression viewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _sysExpressionService.UpdateEquivalentAsync(viewModel.Id, viewModel.Equivalent, User.GetOnlineUser());
            if (result.Type == ServiceResultType.Succeed)
            {
                SysExpressionHelper.Items.Clear();
                _sysExpressionService.FetchAllExpressions();
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
