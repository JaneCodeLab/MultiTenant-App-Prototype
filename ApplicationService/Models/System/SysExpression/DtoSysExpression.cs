
using ApplicationCore.DomainModel;
namespace ApplicationService;

public class DtoSysExpression
{
    public Guid Id { get; set; }
    public Language Language { get; set; }
    public ExpressionTypes ExpressionType { get; set; }
    public int ExpressionItem { get; set; }
    public string? Equivalent { get; set; }
}