

using System.ComponentModel.DataAnnotations;

namespace ApplicationService;

public class BaseFilter
{
    public BaseFilter()
    {
        FilterName = this.GetType().Name;
    }

    public string FilterName { get; set; }
    public bool ClearAll { get; set; } = false;
    public bool ItemRemoved { get; set; } = false;

    public bool? Active { get; set; }
    public bool? Locked { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }


    [DataType(DataType.Date)]
    public DateTime? CreatedAtStart { get; set; }

    [DataType(DataType.Date)]
    public DateTime? CreatedAtEnd { get; set; }


    [DataType(DataType.Date)]
    public DateTime? UpdatedAtStart { get; set; }
    [DataType(DataType.Date)]
    public DateTime? UpdatedAtEnd { get; set; }
}