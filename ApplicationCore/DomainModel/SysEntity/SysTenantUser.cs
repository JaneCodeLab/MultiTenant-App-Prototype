using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.DomainModel;

public class SysTenantUser : BaseEntity<int>
{
    [Required]
    public int TenantId { get; set; }
    public SysTenant? Tenant { get; set; }

    [Required]
    public string UserId { get; set; } = null!;
    public SysCustomUser? User { get; set; }
}