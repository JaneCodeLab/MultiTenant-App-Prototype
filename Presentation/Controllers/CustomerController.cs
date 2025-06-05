
using ApplicationCore.DomainModel;
using ApplicationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

//[Authorize(Roles = $"{Roles.DevPersonnel},{Roles.Admin},{Roles.CompanyAdmin}")]
[Authorize]
public class CustomerController : BaseController
{
    private readonly ILogger<CustomerController> _logger;
    private readonly ICustomerService _customerService;

    public CustomerController(ILogger<CustomerController> logger,
                             IDataProtectionProvider dataProtectionProvider,
                             ICustomerService customerService)
                        : base(dataProtectionProvider)
    {
        _logger = logger;
        _customerService = customerService;
    }

    public async Task<IActionResult> Index(BaseFilter filter)
    {
        filter = GetLatestFilter(filter);

        var model = new ViewListModel<Customer, BaseFilter>
        {
            Records = await _customerService.GetAllAsync(filter, User.GetLoggedInUserTimezoneId()),
            Filter = filter
        };

        return View(model);
    }

    [HttpPost]
    public async Task<JsonResult> Save(Customer model)
    {
        ServiceResult result;
        model.Title = model.Title?.Trim() ?? string.Empty;
        if (model.Id > 0)
            result = await _customerService.CustomUpdateAsync(model, User.GetOnlineUser());
        else
            result = await _customerService.CreateAsync(model, User.GetOnlineUser());

        return new JsonResult(result);
    }

    [HttpPost]
    public async Task<JsonResult> Delete(int id) => new JsonResult(await _customerService.PermanentDeleteAsync(User.GetOnlineUser(), id));

    public async Task<IActionResult> ChangeState(int id, bool activate)
    {
        await _customerService.ChangeActivateAsync(id, !activate, User.GetOnlineUser());
        return RedirectToAction(nameof(Index));
    }
}