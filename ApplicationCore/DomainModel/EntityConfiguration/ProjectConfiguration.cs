using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationCore.DomainModel
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasOne(d => d.Customer)
                       .WithMany(p => p.Projects)
                       .HasForeignKey(d => d.CustomerId)
                       .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
