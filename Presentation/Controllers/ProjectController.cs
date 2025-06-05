
using ApplicationCore.DomainModel;
using ApplicationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Controllers;

//[Authorize(Roles = $"{Roles.DevPersonnel},{Roles.Admin},{Roles.CompanyAdmin}")]
[Authorize]
public class ProjectController : BaseController
{
    private readonly ILogger<ProjectController> _logger;
    private readonly ICustomerService _customerService;
    private readonly IProjectService _projectService;

    public ProjectController(ILogger<ProjectController> logger,
                             IDataProtectionProvider dataProtectionProvider,
                             ICustomerService customerService,
                             IProjectService projectService)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _projectService = projectService;
        _customerService = customerService;
    }

    public async Task<IActionResult> Index(BaseFilter filter)
    {
        filter = GetLatestFilter(filter);

        var model = new ViewListModel<Project, BaseFilter>
        {
            Records = await _projectService.GetAllAsync(filter, User.GetLoggedInUserTimezoneId()),
            Filter = filter
        };
        ViewData["CustomerList"] = new SelectList(await _customerService.GetAllForSelect(c => c.Title, true), nameof(Customer.Id), nameof(Customer.Title));

        return View(model);
    }

    [HttpPost]
    public async Task<JsonResult> Save(Project model)
    {
        ServiceResult result;
        model.Title = model.Title?.Trim() ?? string.Empty;
        if (model.Id != Guid.Empty)
            result = await _projectService.CustomUpdateAsync(model, User.GetOnlineUser());
        else
        {
            model.Id = Guid.NewGuid();
            result = await _projectService.CreateAsync(model, User.GetOnlineUser());
        }

        return new JsonResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _projectService.PermanentDeleteAsync(User.GetOnlineUser(), id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ChangeState(Guid id, bool activate)
    {
        await _projectService.ChangeActivateAsync(id, !activate, User.GetOnlineUser());
        return RedirectToAction(nameof(Index));
    }
}