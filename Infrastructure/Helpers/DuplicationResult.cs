
using ApplicationCore.DomainModel;

namespace Infrastructure.Helpers;

public class DuplicationResult
{
    public bool IsDuplicated { get { return DuplicatedFields.Any(); } }
    public ExpressionTypes ExpressionType { get; set; }
    public List<int> DuplicatedFields { get; set; } = new();

    public DuplicationResult() { }
    public DuplicationResult(ExpressionTypes expressionType, List<int> fields)
    {
        ExpressionType = expressionType;
        DuplicatedFields = fields;
    }
}