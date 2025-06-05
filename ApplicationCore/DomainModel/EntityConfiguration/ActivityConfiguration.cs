using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationCore.DomainModel
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder
                .Property(p => p.Duration)
                .HasComputedColumnSql($"((DATEDIFF(MINUTE, {nameof(Activity.StartDate)} , {nameof(Activity.EndDate)})))");
        }
    }
}
