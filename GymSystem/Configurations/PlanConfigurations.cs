using GymSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.Configurations
{
    public class PlanConfigurations : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(p => p.Description)
                .HasMaxLength(200);

            builder.Property(p => p.Price)
                .HasPrecision(10, 2);

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.IsActive)
                .HasDefaultValue(true);

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint(
                    "CK_Plan_DurationDays",
                    "DurationDays BETWEEN 1 AND 365");
            });

        }
    }
}
