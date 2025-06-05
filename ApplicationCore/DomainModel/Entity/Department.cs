
namespace ApplicationCore.DomainModel;

public class Department : BaseEntity<int>
{
    public string Title { get; set; } = string.Empty;
}