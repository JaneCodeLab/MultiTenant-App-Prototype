
using ApplicationCore;
using ApplicationCore.DomainModel;
using ApplicationService;
using Infrastructure.SqlServerAdapter;

namespace Presentation;

public static class ServicesExtentions
{
    public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork<TenantDbContext>, UnitOfWork<TenantDbContext>>();
        services.AddTransient<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>();

        services.AddTransient<ISysCustomUserService, SysCustomUserService>();
        services.AddTransient<ISysTenantService, SysTenantService>();
        services.AddTransient<ISysTenantUserService, SysTenantUserService>();
        services.AddTransient<ISysExpressionService, SysExpressionService>();
        services.AddTransient<ICaptchaService, CaptchaService>();
        services.AddTransient<ISysExceptionService, SysExceptionService>();
        services.AddTransient<ISysFaqService, SysFaqService>();
        services.AddTransient<ISysHelpService, SysHelpService>();
        services.AddTransient<ISysLogService, SysLogService>();
        services.AddTransient<ISysApiLogService, SysApiLogService>();
        services.AddTransient<ISysReleaseNoteService, SysReleaseNoteService>();
        services.AddTransient<ISysSmtpService, SysSmtpService>();
        services.AddTransient<ISysParameterService, SysParameterService>();

        services.AddTransient<ILogService, LogService>();
        services.AddTransient<IApiLogService, ApiLogService>();
        services.AddTransient<ISmtpService, SmtpService>();

        services.AddTransient<IDepartmentService, DepartmentService>();
        services.AddTransient<ICustomerService, CustomerService>();
        services.AddTransient<IProjectService, ProjectService>();
        services.AddTransient<IDepartmentCustomerService, DepartmentCustomerService>();
        services.AddTransient<IDepartmentMemberService, DepartmentMemberService>();
        services.AddTransient<IDepartmentRoleService, DepartmentRoleService>();
        services.AddTransient<ISprintService, SprintService>();
        services.AddTransient<IActivityService, ActivityService>();
        services.AddTransient<IIssueService, IssueService>();
        services.AddTransient<ISprintTaskService, SprintTaskService>();
        services.AddTransient<ITaskAssigneeService, TaskAssigneeService>();
        services.AddTransient<IBacklogService, BacklogService>();
        services.AddTransient<ISprintPlanService, SprintPlanService>();

        return services;
    }

    public static IServiceProvider FetchDefaultParameters(this IServiceProvider services)
    {
        var expressionService = services.GetService<ISysExpressionService>();
        var parametersService = services.GetService<ISysParameterService>();
        var tenantService = services.GetService<ISysTenantService>();
        var releaseNoteService = services.GetService<ISysReleaseNoteService>();

        parametersService.FetchAllParameters();
        expressionService.FetchAllExpressions();
        tenantService.FetchHelperTenants();
        GeneralVariables.LatestVersionNo = releaseNoteService.GetLatestVersionNo().Result;

        return services;
    }
}