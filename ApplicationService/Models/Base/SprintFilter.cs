
using System.ComponentModel.DataAnnotations;

namespace ApplicationService;

public class SprintFilter : BaseFilter
{
    public SprintFilter()
    {
    }

    public bool Backlog { get; set; } = false;

    [DataType(DataType.Date)]
    public DateTime? StartDateBefore { get; set; }

    [DataType(DataType.Date)]
    public DateTime? StartDateAfter { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime? EndDateBefore { get; set; }

    [DataType(DataType.Date)]
    public DateTime? EndDateAfter { get; set; }
}