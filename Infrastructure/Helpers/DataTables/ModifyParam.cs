
using System.Linq.Expressions;

namespace Infrastructure.Helpers.DataTables;

internal class ModifyParam : ExpressionVisitor
{
    private ParameterExpression _replace;

    public ModifyParam(ParameterExpression p)
    {
        _replace = p;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        return _replace;
    }

}
