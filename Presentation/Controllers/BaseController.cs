
using ApplicationCore;
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.Helpers;
using Infrastructure.Helpers.DataTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace Presentation.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        private IDataProtector _dataProtector;
        private const string CookieLatestFilterdForm = "LFF";
        private const string CookieFilterObjectModel = "FOM";


        public BaseController(IDataProtectionProvider dataProtectionProvider, IServiceProvider? serviceProvider = null)
        {
            _dataProtector = dataProtectionProvider.CreateProtector(GetPageName());
        }

        public async Task<IActionResult> GetPagedData<T, BT, FILTER, DTO, CT>(IBaseService<T, BT, CT> service,
                    Expression<Func<T, DTO>> select
                    ) where T : BaseEntity<BT> where DTO : class, new()
                    where FILTER : BaseFilter, new()
                    where CT : DbContext
        {
            var filter = GetLatestFilter<FILTER>();

            var pagination = new DatatablesParser<T, DTO>(Request.Form).Parse();

            var data = await service.GetAllPagedAsync(filter, pagination, select, User.GetLoggedInUserTimezoneId(), User.GetLoggedInUserLanguageEnum());
            int recordsTotal = data.TotalCount;

            return Ok(new DataTableResults<DTO>(pagination.Draw, recordsTotal, recordsTotal, data));

        }

        public async Task<IActionResult> GetPagedData<T, BT, FILTER, CT>(IBaseService<T, BT, CT> service) where T : BaseEntity<BT>, new() where FILTER : BaseFilter, new() where CT : DbContext
            => await GetPagedData<T, BT, FILTER, T, CT>(service, s => s);

        public T GetLatestFilter<T>(T filter) where T : BaseFilter, new()
        {
            if (filter.IsNullOrEmpty())
            {
                if (Request != null && Request.Cookies[CookieLatestFilterdForm]?.ToString() == GetPageName())
                {
                    if (filter.ClearAll || filter.ItemRemoved)
                    {
                        filter.ClearAll = false;
                        Response.Cookies.Delete(CookieLatestFilterdForm);
                        Response.Cookies.Delete(CookieFilterObjectModel);
                    }
                    else
                    {
                        filter = GetFilterFromCookie<T>(filter);
                    }
                }
                else if (Request != null)
                {
                    Response.Cookies.Delete(CookieLatestFilterdForm);
                    Response.Cookies.Delete(CookieFilterObjectModel);
                }
            }
            else
            {
                Response.Cookies.Append(CookieLatestFilterdForm, this.GetType().Name.Replace("Controller", "Page").ToLower(), new CookieOptions { Expires = DateTime.UtcNow.AddMinutes(30) });
                Response.Cookies.Append(CookieFilterObjectModel, _dataProtector.Protect(JObject.FromObject(filter).ToString()), new CookieOptions { Expires = DateTime.UtcNow.AddMinutes(30) });
            }
            return filter;
        }
        public T GetLatestFilter<T>() where T : BaseFilter, new()
        {
            return GetFilterFromCookie<T>();
        }

        private T GetFilterFromCookie<T>(T? filter = default) where T : BaseFilter, new()
        {
            filter = filter is null ? new() : filter;
            if (Request != null && Request.Cookies[CookieLatestFilterdForm]?.ToString() == GetPageName())
            {
                var coockie = _dataProtector.Unprotect(Request.Cookies[CookieFilterObjectModel].ToString());
                if (!string.IsNullOrEmpty(coockie))
                {
                    var cookieFilter = JObject.Parse(coockie).ToObject<T>() ?? new BaseFilter();
                    if (!cookieFilter.IsNullOrEmpty())
                        filter = (T)cookieFilter;
                }
            }
            return filter;
        }

        private string GetPageName() => this.GetType().Name.Replace("Controller", "Page").ToLower();

        internal IActionResult RedirectToLock(string returnUrl)
        {
            //ToDo : Check returnUrl
            return RedirectToAction("Lock", nameof(ErrorsController).Replace("Controller", ""), new { returnUrl = returnUrl });
        }
        internal IActionResult AccessDenied(string returnUrl)
        {
            //ToDo : Check returnUrl
            return RedirectToAction("AccessDenied", nameof(ErrorsController).Replace("Controller", ""), new { returnUrl = returnUrl });
        }
    }

}
