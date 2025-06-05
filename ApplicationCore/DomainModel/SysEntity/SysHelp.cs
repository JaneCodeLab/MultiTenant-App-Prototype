
namespace ApplicationCore.DomainModel;

public class SysHelp : BaseEntity<Guid>
{
    public SysHelp() => Id = Guid.NewGuid();

    public int Code { get; set; }
    public string ControlerName { get; set; } = null!;
    public string ViewName { get; set; } = null!;
    public string Content { get; set; } = null!;
    public Language Language { get; set; }
}