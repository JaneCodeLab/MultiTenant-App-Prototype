
namespace ApplicationCore.DomainModel;

public class Customer : BaseEntity<int>
{
    public Customer() => Projects = new HashSet<Project>();
    public string Title { get; set; } = string.Empty;

    public ICollection<Project> Projects { get; set; }
}