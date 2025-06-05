
namespace ApplicationCore;

public class MainColumnAttribute : Attribute
{
    public string OrderBy { get; set; } = string.Empty;
}
