using ApplicationService;

namespace ApiService.Helper;
public static class Initializer
{
    public static async Task FetchDefaultParameters(IServiceCollection servicesCollection)
    {
        var services = servicesCollection.BuildServiceProvider();

        var parameterService = services.GetService<ISysParameterService>();
        var expressionService = services.GetService<ISysExpressionService>();
        var tenantService = services.GetService<ISysTenantService>();

        await Task.Run(() => tenantService.FetchHelperTenants());
        await Task.Run(() => parameterService.FetchAllParameters());
        await Task.Run(() => expressionService.FetchAllExpressions());
    }
}