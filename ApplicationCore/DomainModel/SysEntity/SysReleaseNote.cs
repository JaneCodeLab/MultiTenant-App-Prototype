
namespace ApplicationCore.DomainModel;

public class SysReleaseNote : BaseEntity<int>
{
    public string ReleaseNo { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public bool IsComing { get; set; }
    public string? Note { get; set; }
}