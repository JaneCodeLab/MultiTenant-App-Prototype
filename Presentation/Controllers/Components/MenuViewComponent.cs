using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly UserManager<SysCustomUser> _userManager;

        public MenuViewComponent(UserManager<SysCustomUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(string currentMenu, ClaimsPrincipal user)
        {
            var claims = user.Claims;
            var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var result = new List<VmMenuItem>();
            var langEnum = user.GetLoggedInUserLanguageEnum();
            if (User.IsInRole(Roles.Admin))
            {
                var language = new VmMenuItem
                {
                    Order = 100,
                    Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.SystemLanguage.ToInt()),
                    Icon = "fas fa-language",
                    Selected = false,
                    Children = new List<VmMenuItem> {
                                    new VmMenuItem { Order = 110, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Parameters.ToInt()), Controller = "SysParameter", Action = "Index", Icon = "fas fa-list"},
                                    new VmMenuItem { Order = 120, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Expressions.ToInt()), Controller = "SysExpression", Action = "Index", Icon = "fas fa-language"},
                                    new VmMenuItem { Order = 130, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.ClearCaches.ToInt()), Controller = "SysCacheManagement", Action = "ClearCache", Icon = "fas fa-broom"},
                                }
                };

                var systemSettings = new VmMenuItem
                {
                    Order = 1000,
                    Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.SystemSettings.ToInt()),
                    Icon = "fas fa-gears",
                    Selected = false,
                    Children = new List<VmMenuItem> {
                                    new VmMenuItem { Order = 1010, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Employees.ToInt()), Controller = "SysEmployee", Action = "Index", Icon = "fas fa-user"},
                                    new VmMenuItem { Order = 1020, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Users.ToInt()), Controller = "SysUserManagement", Action = "Index", Icon = "fas fa-user"},
                                    new VmMenuItem { Order = 1030, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Roles.ToInt()), Controller = "SysRoleUser", Action = "Index", Icon = "fas fa-user-tag"},
                                    new VmMenuItem { Order = 1050, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.ReleaseNote.ToInt()), Controller = "SysReleaseNote", Action = "Index", Icon = "fas fa-note-sticky"},
                                }
                };
                var systemContents = new VmMenuItem
                {
                    Order = 800,
                    Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.SystemContents.ToInt()),
                    Icon = "fas fa-book-open",
                    Selected = false,
                    Children = new List<VmMenuItem> {
                                    new VmMenuItem { Order = 820, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Faq.ToInt()), Controller = "SysFaq", Action = "Index", Icon = "fas fa-comments"},
                                    new VmMenuItem { Order = 830, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Help.ToInt()), Controller = "SysHelp", Action = "Index", Icon = "fas fa-circle-question"},
                                }
                };

                var systemLogs = new VmMenuItem
                {
                    Order = 900,
                    Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.SystemLogs.ToInt()),
                    Icon = "fas fa-box-archive",
                    Selected = false,
                    Children = new List<VmMenuItem> {
                                    new VmMenuItem { Order = 910, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Bugs.ToInt()), Controller = "SysException", Action = "Index", Icon = "fas fa-bug"},
                                    new VmMenuItem { Order = 920, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.AuditLogs.ToInt()), Controller = "SysLog", Action = "Index", Icon = "fas fa-table-list"},
                                    new VmMenuItem { Order = 925, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.TenantLogs.ToInt()), Controller = "SysTenantLog", Action = "Index", Icon = "fas fa-table-list"},
                                    new VmMenuItem { Order = 930, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.ApiLogs.ToInt()), Controller = "SysApiLog", Action = "Sent", Icon = "fas fa-right-from-bracket"},
                                }
                };

                result.Add(language);
                result.Add(systemSettings);
                result.Add(systemContents);
                result.Add(systemLogs);
            }
            else
            {
                var setting = new VmMenuItem
                {
                    Order = 100,
                    Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Settings.ToInt()),
                    Icon = "fas fa-gear",
                    Selected = false,
                    Children = new List<VmMenuItem> {
                                    new VmMenuItem { Order = 100, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Personel.ToInt()), Controller = "Personel", Action = "Index", Icon = "fas fa-user"},
                                    new VmMenuItem { Order = 101, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Customers.ToInt()), Controller = "Customer", Action = "Index", Icon = "fas fa-user-secret"},
                                    new VmMenuItem { Order = 102, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Departments.ToInt()), Controller = "Department", Action = "Index", Icon = "fas fa-people-group"},
                                    new VmMenuItem { Order = 103, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Projects.ToInt()), Controller = "Project", Action = "Index", Icon = "fas fa-briefcase"},
                                }
                };
                var departmentSettings = new VmMenuItem
                {
                    Order = 200,
                    Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.DepartmentSettings.ToInt()),
                    Icon = "fas fa-people-group",
                    Selected = false,
                    Children = new List<VmMenuItem> {
                                    new VmMenuItem { Order = 200, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Members.ToInt()), Controller = "DepartmentMember", Action = "Index", Icon = "fas fa-user"},
                                    new VmMenuItem { Order = 201, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.DepartmentCustomers.ToInt()), Controller = "DepartmentCustomer", Action = "Index", Icon = "fas fa-user-secret"},
                                    new VmMenuItem { Order = 202, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.DepartmentRoles.ToInt()), Controller = "DepartmentRole", Action = "Index", Icon = "fas fa-user-tag"},
                                }
                };


                var Scrum = new VmMenuItem
                {
                    Order = 300,
                    Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Scrum.ToInt()),
                    Icon = "fas fa-handshake-simple",
                    Selected = false,
                    Children = new List<VmMenuItem> {
                                    new VmMenuItem { Order = 300, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Sprints.ToInt()), Controller = "Sprint", Action = "Index", Icon = "fas fa-calendar-days"},
                                    new VmMenuItem { Order = 301, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Backlog.ToInt()), Controller = "Backlog", Action = "Index", Icon = "fas fa-list-check"},
                                    new VmMenuItem { Order = 302, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.CurrentSprint.ToInt()), Controller = "CurrentSprint", Action = "Index", Icon = "fas fa-calendar-week"},
                                    new VmMenuItem { Order = 401, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.SprintReview.ToInt()), Controller = "SprintReview", Action = "Index", Icon = "fas fa-calendar-check"},
                                }
                };

                var Report = new VmMenuItem
                {
                    Order = 400,
                    Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.Report.ToInt()),
                    Icon = "fas fa-chart-pie",
                    Selected = false,
                    Children = new List<VmMenuItem> {
                                    new VmMenuItem { Order = 401, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.SprintReviewReport.ToInt()), Controller = "SprintReviewReport", Action = "Index", Icon = "fas fa-calendar-check"},
                                }
                };


                var MyTasks = new VmMenuItem
                {
                    Order = 401,
                    Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.MyPanel.ToInt()),
                    Icon = "far fa-calendar-plus",
                    Selected = false,
                    Children = new List<VmMenuItem> {
                                    new VmMenuItem { Order = 410, Title = SysExpressionHelper.Get(langEnum, ExpressionTypes.SysMenu, SysMenuExpression.MyTasks.ToInt()), Controller = "MyTasks", Action = "Index", Icon = "fas fa-list-check"},
                                }
                };

                result.Add(departmentSettings);
                result.Add(setting);
                result.Add(Scrum);
                result.Add(Report);
                result.Add(MyTasks);

            }



            if (result.Any(c => c.Children.Any(e => e.Title == currentMenu)))
                result.FirstOrDefault(c => c.Children.Any(e => e.Title == currentMenu)).Children.FirstOrDefault(e => e.Title == currentMenu).Selected = true;

            if (result.Any(c => c.Children.Any(e => e.Selected)))
                result.FirstOrDefault(c => c.Children.Any(e => e.Selected)).Selected = true;
            else
                result.OrderBy(o => o.Order).FirstOrDefault().Selected = true;


            return View(result);
        }
    }
}
