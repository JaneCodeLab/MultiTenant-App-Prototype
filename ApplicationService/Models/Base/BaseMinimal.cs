
namespace ApplicationService;

public class BaseMinimal<BT>
{
    public BT Id { get; set; } = default(BT)!;
    public bool Active { get; set; }
    public bool Locked { get; set; }
}