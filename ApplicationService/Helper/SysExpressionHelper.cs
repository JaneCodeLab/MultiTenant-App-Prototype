using ApplicationCore.DomainModel;
using Infrastructure.Helpers;
using System.Reflection;

namespace ApplicationService;

public static class SysExpressionHelper
{
    public static List<DtoSysExpression> Items = new();

    public static string Get(Language language, ExpressionTypes expressionType, int expressionItem)
    {
        return Items?.FirstOrDefault(s => s.ExpressionType == expressionType &&
                                                  s.ExpressionItem == expressionItem &&
                                                  s.Language == language)?.Equivalent ?? FindPureName(expressionType, expressionItem);
    }


    public static List<DtoSysExpression> GetAll(Language language, ExpressionTypes expressionType)
                         => Items.Where(s => s.ExpressionType == expressionType && s.Language == language).ToList();

    public static string Get(Language language,
                             ExpressionTypes expressionType,
                             int parameterItem,
                             Dictionary<string, object> replacements)
                         => Get(language, expressionType, parameterItem).Replace(replacements);


    public static string Get(Language language,
                             ExpressionTypes expressionType,
                             int expressionItem,
                             (string key, object value) replacement)
                         => Get(language, expressionType, expressionItem).Replace(new[] { replacement }.ToDictionary(kvp => kvp.key, kvp => kvp.value));

    public static string FindPureName(ExpressionTypes expressionType, int expressionItem)
    {
        var relatedEnum = typeof(ExpressionTypes).GetField(expressionType.ToString())?.GetCustomAttribute<RelatedEnum>();
        return Enum.Parse(relatedEnum.EnumType, expressionItem.ToString())?.ToString() ?? "Wrong";
    }
}