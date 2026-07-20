using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.DAL.Configurations
{
    internal class PlanConfigurations : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.ToTable("Plans", table =>
            {
                table.HasCheckConstraint(
                    "CK_Plan_DurationDays",
                    "DurationDays BETWEEN 1 AND 365");

                table.HasCheckConstraint(
                    "CK_Plan_Price",
                    "Price > 0");
            });

            builder.Property(x => x.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.Description)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(x => x.DurationDays)
                   .IsRequired();

            builder.Property(x => x.Price)
                   .HasPrecision(10, 2);


            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.UpdatedAt);

            builder.HasMany(x => x.Memberships)
                   .WithOne(x => x.Plan)
                   .HasForeignKey(x => x.PlanId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Name)
                   .IsUnique();

        }
    }
}
