
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Controllers;

//[Authorize(Roles = $"{Roles.DevPersonnel},{Roles.Admin},{Roles.CompanyAdmin}")]
[Authorize]
public class DepartmentCustomerController : BaseController
{
    private readonly ILogger<DepartmentCustomerController> _logger;
    private readonly ICustomerService _customerService;
    private readonly IDepartmentService _departmentService;
    private readonly IDepartmentCustomerService _departmentCustomerService;

    public DepartmentCustomerController(ILogger<DepartmentCustomerController> logger,
                             IDataProtectionProvider dataProtectionProvider,
                             IDepartmentCustomerService departmentCustomerService,
                             IDepartmentService departmentService,
                             ICustomerService customerService)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _departmentService = departmentService;
        _customerService = customerService;
        _departmentCustomerService = departmentCustomerService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["CustomerList"] = new SelectList(await _customerService.GetAllForSelect(c => c.Title, true), nameof(Customer.Id), nameof(Customer.Title));
        return View(await _departmentCustomerService.GetDepartmentCustomer((User.GetLoggedInUserSquad()).ToInt()));
    }

    [HttpPost]
    public async Task<JsonResult> Save(DepartmentCustomer model)
    {
        model.DepartmentId = (User.GetLoggedInUserSquad()).ToInt();

        ServiceResult result;
        if (model.Id > 0)
            result = await _departmentCustomerService.CustomUpdateAsync(model, User.GetOnlineUser());
        else
            result = await _departmentCustomerService.CreateAsync(model, User.GetOnlineUser());

        return new JsonResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _departmentCustomerService.PermanentDeleteAsync(User.GetOnlineUser(), id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ChangeState(int id, bool activate)
    {
        await _departmentCustomerService.ChangeActivateAsync(id, !activate, User.GetOnlineUser());
        return RedirectToAction(nameof(Index));
    }
}