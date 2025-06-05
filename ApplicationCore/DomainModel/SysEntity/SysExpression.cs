
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.DomainModel;

public class SysExpression : BaseEntity<Guid>
{
    public SysExpression() => Id = Guid.NewGuid();

    [Required]
    public Language Language { get; set; }

    [Required]
    public ExpressionTypes ExpressionType { get; set; }

    public int ExpressionItem { get; set; }
    public string? Equivalent { get; set; }
}