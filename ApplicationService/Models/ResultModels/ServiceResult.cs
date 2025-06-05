
using ApplicationCore;
using ApplicationCore.DomainModel;

namespace ApplicationService;

public class ServiceResult
{
    public Language Language { get; set; }
    public ExpressionTypes ExpressionType { get; set; }

    public int ExpressionItem { get; set; }
    public ServiceResultType Type { get; set; }
    public string? Message
    {
        get
        {
            if (!Parameters.Any())
                return SysExpressionHelper.Get(Language, ExpressionType, ExpressionItem);

            if (Parameters.Any(c => c.Key == ServiceResultParameterType.RefrenceCode))
                return SysExpressionHelper.Get(Language, ExpressionType, ExpressionItem, (ExpressionParamConstants.Code, Parameters[ServiceResultParameterType.RefrenceCode]));

            if (Parameters.Any(c => c.Key == ServiceResultParameterType.DuplicatedFieldName))
                return SysExpressionHelper.Get(Language, ExpressionType, ExpressionItem, (ExpressionParamConstants.Field, Parameters[ServiceResultParameterType.DuplicatedFieldName]));

            return SysExpressionHelper.Get(Language, ExpressionType, ExpressionItem);
        }
    }

    public Dictionary<string, string> Parameters = new();
}