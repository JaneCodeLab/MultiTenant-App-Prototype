
using ApplicationCore.DomainModel;
using ApplicationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize(Roles = Roles.DevPersonnel + "," + Roles.Admin)]
public class SysReleaseNoteController : BaseController
{
    private readonly ILogger<SysReleaseNoteController> _logger;
    private readonly ISysReleaseNoteService _releaseNoteService;

    public SysReleaseNoteController(ILogger<SysReleaseNoteController> logger,
                             IDataProtectionProvider dataProtectionProvider,
                             ISysReleaseNoteService releaseNoteService)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _releaseNoteService = releaseNoteService;
    }

    public async Task<IActionResult> Index(BaseFilter filter)
    {
        filter = GetLatestFilter(filter);

        var model = new ViewListModel<SysReleaseNote, BaseFilter>
        {
            Records = await _releaseNoteService.GetAllAsync(filter, User.GetLoggedInUserTimezoneId()),
            Filter = filter
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {

        return View(new SysReleaseNote() { ReleaseDate = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SysReleaseNote model)
    {
        if (ModelState.IsValid)
        {
            var result = await _releaseNoteService.CreateAsync(model, User.GetOnlineUser());
            if (result.Type == ServiceResultType.Succeed)
                return RedirectToAction(nameof(Index));
            else
            {
                ModelState.AddModelError(nameof(model.ReleaseNo), result.Message);
                return View(model);
            }
        }
        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _releaseNoteService.GetAsync(id);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SysReleaseNote model)
    {
        if (ModelState.IsValid)
        {
            var result = await _releaseNoteService.UpdateAsync(model, User.GetOnlineUser());
            if (result.Type == ServiceResultType.Succeed)
                return RedirectToAction(nameof(Index));
            else
            {
                ModelState.AddModelError(nameof(model.ReleaseNo), result.Message);
                return View(model);
            }
        }
        return View(model);
    }
    public async Task<IActionResult> Detail(int id)
    {
        var model = await _releaseNoteService.GetAsync(id);
        return View(model);
    }

    public async Task<IActionResult> Delete(int id, string message)
    {
        if (id == 0)
            return NotFound();

        var model = await _releaseNoteService.GetAsync(id);
        if (model.Locked)
            return RedirectToLock(Request.Path);

        if (model == null)
            return NotFound();

        ViewBag.message = message;
        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _releaseNoteService.PermanentDeleteAsync(User.GetOnlineUser(), id);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            var message = $"There is a problem";
            return RedirectToAction(nameof(Delete), new { id, message });
        }
    }
    public async Task<IActionResult> ChangeState(int id, bool activate)
    {
        await _releaseNoteService.ChangeActivateAsync(id, !activate, User.GetOnlineUser());
        return RedirectToAction(nameof(Index));
    }
}