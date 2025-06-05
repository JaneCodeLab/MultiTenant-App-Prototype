
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.SqlServerAdapter
{
    public class TenantProvider
    {
        private TenantConfig tenantConfig;

        public bool IsTenantValid { get { return tenantConfig != null; } }

        public TenantProvider()
        {

        }
        public TenantProvider(int tenantId)
        {
            if (tenantId > 0)
                tenantConfig = TenantHelper.Get(tenantId);
        }
        public TenantProvider(IHttpContextAccessor contextAccessor)
        {
            var context = contextAccessor.HttpContext;
            var tenantId = context.User.Claims.FirstOrDefault(c => c.Type == CustomClaimType.TenantId)?.Value.ToInt() ?? 0;
            if (tenantId > 0)
                tenantConfig = TenantHelper.Get(tenantId);
        }

        public string GetConnectionString()
        {
            if (!IsTenantValid)
                throw new InvalidOperationException("Tenant Is Not Valid Or Not Exist!");

            if (tenantConfig.DbUsername.IsNullOrEmpty() && tenantConfig.DbPassword.IsNullOrEmpty())
                return $"Server={tenantConfig.DbIp};Database=PmaTenant_{tenantConfig.TenantId};Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";
            else
                return $"Data Source={tenantConfig.DbIp};Initial Catalog=PmaTenant_{tenantConfig.TenantId};Persist Security Info=True;TrustServerCertificate=True;User ID={tenantConfig.DbUsername};Password={tenantConfig.DbPassword}";
        }
    }
}
