
using ApplicationCore.DomainModel;
using ApplicationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize(Roles = Roles.DevPersonnel + "," + Roles.Admin)]
public class SysHelpController : BaseController
{
    private readonly ILogger<SysHelpController> _logger;
    private readonly ISysHelpService _helpService;

    public SysHelpController(ILogger<SysHelpController> logger,
                             IDataProtectionProvider dataProtectionProvider,
                             ISysHelpService helpService)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _helpService = helpService;
    }

    public async Task<IActionResult> Index(BaseFilter filter)
    {
        filter = GetLatestFilter(filter);

        var model = new ViewListModel<SysHelp, BaseFilter>
        {
            Records = await _helpService.GetAllAsync(filter, User.GetLoggedInUserTimezoneId()),
            Filter = filter
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {

        return View(new SysHelp());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SysHelp model)
    {
        model.Id = new Guid();
        if (ModelState.IsValid)
        {
            var result = await _helpService.CreateAsync(model, User.GetOnlineUser());
            if (result.Type == ServiceResultType.Succeed)
                return RedirectToAction(nameof(Index));
            else
            {
                ModelState.AddModelError(nameof(model.Code), result.Message);
                return View(model);
            }
        }
        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var model = await _helpService.GetAsync(id);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SysHelp model)
    {
        if (ModelState.IsValid)
        {
            var result = await _helpService.UpdateAsync(model, User.GetOnlineUser());
            if (result.Type == ServiceResultType.Succeed)
                return RedirectToAction(nameof(Index));
            else
            {
                ModelState.AddModelError(nameof(model.Code), result.Message);
                return View(model);
            }
        }
        return View(model);
    }
    public async Task<IActionResult> Detail(Guid id)
    {
        var model = await _helpService.GetAsync(id);
        return View(model);
    }

    public async Task<IActionResult> Delete(Guid id, string message)
    {
        if (id == Guid.Empty)
            return NotFound();

        var model = await _helpService.GetAsync(id);
        if (model.Locked)
            return RedirectToLock(Request.Path);

        if (model == null)
            return NotFound();

        ViewBag.message = message;
        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            await _helpService.PermanentDeleteAsync(User.GetOnlineUser(), id);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            var message = $"There is a problem";
            return RedirectToAction(nameof(Delete), new { id, message });
        }
    }
    public async Task<IActionResult> ChangeState(Guid id, bool activate)
    {
        await _helpService.ChangeActivateAsync(id, !activate, User.GetOnlineUser());
        return RedirectToAction(nameof(Index));
    }
}