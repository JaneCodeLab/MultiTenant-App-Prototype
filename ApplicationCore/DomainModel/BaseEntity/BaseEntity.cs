namespace ApplicationCore.DomainModel;

public class BaseEntity<BT>
{
    public BT Id { get; set; } = default(BT)!;
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? CreatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public string? UpdatedById { get; set; }
    public bool Locked { get; set; } = false;
    public bool Active { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public string? Owner { get; set; }
}