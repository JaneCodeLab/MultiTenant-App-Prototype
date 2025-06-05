
using ApplicationCore.DomainModel;

namespace Presentation
{
    public class VmTenantUser
    {
        public string UserId { get; set; }
        public List<SysTenant> Tenants { get; set; } = new List<SysTenant>();
    }
}