
namespace Infrastructure.Helpers.DataTables;

public class DataTableConstants
{
    public const string COLUMN_PROPERTY_PATTERN = @"columns\[(\d+)\]\[data\]";
    public const string ORDER_PATTERN = @"order\[(\d+)\]\[column\]";

    public const string DISPLAY_START = "start";
    public const string DISPLAY_LENGTH = "length";
    public const string DRAW = "draw";
    public const string ASCENDING_SORT = "asc";
    public const string SEARCH_KEY = "search[value]";
    public const string SEARCH_REGEX_KEY = "search[regex]";

    public const string DATA_PROPERTY_FORMAT = "columns[{0}][data]";
    public const string SEARCHABLE_PROPERTY_FORMAT = "columns[{0}][searchable]";
    public const string ORDERABLE_PROPERTY_FORMAT = "columns[{0}][orderable]";
    public const string SEARCH_VALUE_PROPERTY_FORMAT = "columns[{0}][search][value]";
    public const string SEARCH_REGEX_PROPERTY_FORMAT = "columns[{0}][search][regex]";
    public const string ORDER_COLUMN_FORMAT = "order[{0}][column]";
    public const string ORDER_DIRECTION_FORMAT = "order[{0}][dir]";
    public const string ORDERING_ENABLED = "ordering";

    public const string CONTAINS_FN = "Contains";
    public const string STARTS_WITH_FN = "StartsWith";
    public const string ENDS_WITH_FN = "EndsWith";

    public const string DEFAULT_STARTS_WITH_TOKEN = "*|";
    public const string DEFAULT_ENDS_WITH_TOKEN = "|*";

    public static string GetKey(string format, string index)
    {
        return string.Format(format, index);
    }
}
