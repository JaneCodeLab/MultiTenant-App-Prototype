
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize(Roles = Roles.DevPersonnel + "," + Roles.Admin)]
public class SysFaqController : BaseController
{
    private readonly ILogger<SysFaqController> _logger;
    private readonly ISysFaqService _faqService;

    public SysFaqController(ILogger<SysFaqController> logger,
                            IDataProtectionProvider dataProtectionProvider,
                            ISysFaqService faqService)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _faqService = faqService;
    }

    public async Task<IActionResult> Index(BaseFilter filter)
    {
        filter = GetLatestFilter(filter);

        var model = new ViewListModel<SysFaq, BaseFilter>
        {
            Records = await _faqService.GetAllAsync(filter, User.GetLoggedInUserTimezoneId()),
            Filter = filter
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new SysFaq());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SysFaq model)
    {
        model.Id = new Guid();
        if (ModelState.IsValid)
        {
            var result = await _faqService.CreateAsync(model, User.GetOnlineUser());
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
        var model = await _faqService.GetAsync(id);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SysFaq model)
    {
        if (ModelState.IsValid)
        {
            var result = await _faqService.UpdateAsync(model, User.GetOnlineUser());
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
        var model = await _faqService.GetAsync(id);
        return View(model);
    }

    public async Task<IActionResult> Delete(Guid id, string message)
    {
        if (id == Guid.Empty)
            return NotFound();

        var model = await _faqService.GetAsync(id);
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
            await _faqService.PermanentDeleteAsync(User.GetOnlineUser(), id);
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
        await _faqService.ChangeActivateAsync(id, !activate, User.GetOnlineUser());
        return RedirectToAction(nameof(Index));
    }
}
