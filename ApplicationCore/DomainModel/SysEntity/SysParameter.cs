
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.DomainModel;

public class SysParameter : BaseEntity<Guid>
{
    public SysParameter() => Id = Guid.NewGuid();

    [Required]
    public Language Language { get; set; }

    [Required]
    public ParameterTypes ParameterType { get; set; }

    public int ParameterItem { get; set; }
    public string? Equivalent { get; set; }
}