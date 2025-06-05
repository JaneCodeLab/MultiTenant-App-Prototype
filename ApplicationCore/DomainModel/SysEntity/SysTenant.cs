
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.DomainModel;

public class SysTenant : BaseEntity<int>
{
    [Required]
    public string Title { set; get; } = null!;
    [Required]
    public string DbIp { get; set; } = null!;
    public string? DbUsername { get; set; }
    public string? DbPassword { get; set; }

    public string? Logo { get; set; }
}