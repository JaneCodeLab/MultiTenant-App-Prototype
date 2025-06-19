using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace ApiService.Middleware;

public class AddHeaderOperationFilter : IOperationFilter
{
    private const string TenantIdHeader = "tenant-id";
    private const string LanguageHeader = "language";
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        bool shouldSkip = context.MethodInfo.GetCustomAttribute<SkipSwaggerHeaderAttribute>() != null
                       || context.MethodInfo.DeclaringType?.GetCustomAttribute<SkipSwaggerHeaderAttribute>() != null;

        if (shouldSkip)
            return;

        operation.Parameters ??= new List<OpenApiParameter>();

        // Add tenant-id header if not already present
        if (!operation.Parameters.Any(p => p.Name == TenantIdHeader && p.In == ParameterLocation.Header))
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = TenantIdHeader,
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema { Type = "string" },
                Description = "The tenant ID header required by the API."
            });
        }

        // Add language header if not already present
        if (!operation.Parameters.Any(p => p.Name == LanguageHeader && p.In == ParameterLocation.Header))
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = LanguageHeader,
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema { Type = "string" },
                Description = "Optional language header for localization."
            });
        }
    }
}
