
namespace ApplicationCore.DomainModel;

public class SysFaq : BaseEntity<Guid>
{
    public SysFaq() => Id = Guid.NewGuid();

    public int Code { get; set; }
    public string ControllerName { get; set; } = null!;
    public string ViewName { get; set; } = null!;
    public string Question { get; set; } = null!;
    public string? Answer { get; set; }
    public Language Language { get; set; }
}