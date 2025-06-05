
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.DomainModel;

[EntityTypeConfiguration(typeof(ActivityConfiguration))]
public class Activity : BaseEntity<Guid>
{
    public string UserId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Duration { get; private set; }

    public Guid SprintTaskId { get; set; }
    public SprintTask? SprintTask { get; set; }

}