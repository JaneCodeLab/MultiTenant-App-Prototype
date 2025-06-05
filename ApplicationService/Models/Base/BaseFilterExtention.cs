
using ApplicationCore.DomainModel;
using Infrastructure.Helpers;
using System.Reflection;

namespace ApplicationService;

public static class BaseFilterExtention
{
    public static bool IsNullOrEmpty(this BaseFilter value)
    {
        if (value != null && value.GetType().GetProperties().Any(prop => prop.Name != nameof(value.ClearAll) &&
                                                        prop.Name != nameof(value.ItemRemoved) &&
                                                        prop.Name != nameof(value.FilterName) &&
                                                        prop.GetValue(value) != null))
            return false;
        return true;
    }

    public static List<PropertyInfo> GetFields(this BaseFilter filter)
    {

        return filter.GetType().GetProperties().Where(prop => prop.Name != nameof(filter.ClearAll) &&
                                                              prop.Name != nameof(filter.ItemRemoved) &&
                                                              prop.Name != nameof(filter.FilterName)).ToList();
    }

    public static string GetLable(this PropertyInfo property, Language language, params ExpressionTypes[] expressionTypes)
    {
        foreach (var expressionType in expressionTypes)
        {
            var relatedEnum = typeof(ExpressionTypes).GetField(expressionType.ToString())?.GetCustomAttribute<RelatedEnum>();

            object? result;
            if (Enum.TryParse(relatedEnum.EnumType, property.Name, out result))
                return SysExpressionHelper.Get(language, expressionType, (int)result);
        }

        return property.Name;
    }
}