using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.DAL.Configurations
{
    internal class MembershipConfigurations : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.ToTable("Memberships", table =>
            {
                table.HasCheckConstraint(
                    "CK_Membership_EndDate",
                    "EndDate > StartDate");
            });

            builder.Property(x => x.StartDate)
                   .IsRequired();

            builder.Property(x => x.EndDate)
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.UpdatedAt);

            builder.HasOne(x => x.Member)
                   .WithMany(x => x.Memberships)
                   .HasForeignKey(x => x.MemberId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Plan)
                   .WithMany(x => x.Memberships)
                   .HasForeignKey(x => x.PlanId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new
            {
                x.MemberId,
                x.StartDate
            });
        }
    }
}
