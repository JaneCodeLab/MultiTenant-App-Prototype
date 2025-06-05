
namespace ApplicationService;

public class BaseMinimalListItem<BT>
{
    public BT Id { get; set; } = default(BT)!;
    public bool Active { get; set; }
    public bool Locked { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; } = null!;
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public string? Owner { get; set; }
}