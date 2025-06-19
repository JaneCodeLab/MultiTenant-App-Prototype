
using ApplicationCore;
using ApplicationService;
using Infrastructure.SqlServerAdapter;

namespace ApiService
{
    public static class ServicesExtentions
    {
        public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork<TenantDbContext>, UnitOfWork<TenantDbContext>>();
            services.AddTransient<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>();

            services.AddTransient<ISysTenantService, SysTenantService>();
            services.AddTransient<ISysApiUserService, SysApiUserService>();
            services.AddTransient<ISysParameterService, SysParameterService>();
            services.AddTransient<ISysExpressionService, SysExpressionService>();

            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ITaskApiService, TaskApiService>();

            return services;
        }

        public static void UseDefaultCorsPolicy(this IApplicationBuilder app)
        {
            app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
        }
    }
}
