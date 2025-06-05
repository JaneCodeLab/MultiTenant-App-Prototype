
namespace ApplicationCore.DomainModel;

public class Sprint : BaseEntity<int>
{
    public string Title { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public bool Current { get; set; } = false;
    public bool Backlog { get; set; } = false;
    public bool Finished { get; set; } = false;

    public int DepartmentId { get; set; }
}