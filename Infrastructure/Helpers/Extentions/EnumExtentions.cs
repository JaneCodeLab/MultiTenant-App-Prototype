
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Infrastructure.Helpers
{
    public static class EnumExtentions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.Name ?? enumValue.ToString();
        }
        public static int ToInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }
    }
}
