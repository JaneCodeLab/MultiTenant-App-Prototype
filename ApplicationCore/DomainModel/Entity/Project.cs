
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.DomainModel;

[EntityTypeConfiguration(typeof(ProjectConfiguration))]
public class Project : BaseEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public int CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }
}