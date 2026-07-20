using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.DAL.Configurations
{
    internal class MemberConfigurations : GymUserConfigurations<Member>, IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            base.Configure(builder);

            builder.ToTable("Members", table =>
            {
                table.HasCheckConstraint(
                    "CK_Member_JoinDate",
                    "JoinDate <= GETDATE()");
            });

            builder.Property(x => x.Photo)
                   .IsRequired()
                   .HasColumnType("varchar")
                   .HasMaxLength(250);

            builder.Property(x => x.JoinDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.HealthRecord)
                   .WithOne(x => x.Member)
                   .HasForeignKey<HealthRecord>(x => x.MemberId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Memberships)
                   .WithOne(x => x.Member)
                   .HasForeignKey(x => x.MemberId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Bookings)
                   .WithOne(x => x.Member)
                   .HasForeignKey(x => x.MemberId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
