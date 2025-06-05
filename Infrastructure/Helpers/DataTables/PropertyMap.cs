
using System.Reflection;

namespace Infrastructure.Helpers.DataTables;

internal class PropertyMap
{
    public PropertyInfo Property { get; set; }
    public bool Orderable { get; set; }
    public bool Searchable { get; set; }
    public string Filter { get; set; }
}


