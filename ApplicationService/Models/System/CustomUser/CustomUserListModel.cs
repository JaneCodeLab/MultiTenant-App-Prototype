
using ApplicationCore.DomainModel;

namespace ApplicationService;

public class CustomUserListModel : BaseMinimalListItem<Guid>
{

    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool EmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public Gender Gender { get; set; } = Gender.Male;
    public string? AgentNames { get; set; }
}