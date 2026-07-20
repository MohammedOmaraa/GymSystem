using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.DAL.Configurations
{
    internal class HealthRecordConfigurations : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("HealthRecords", table =>
            {
                table.HasCheckConstraint(
                    "CK_HealthRecord_Height",
                    "Height > 0");

                table.HasCheckConstraint(
                    "CK_HealthRecord_Weight",
                    "Weight > 0");
            });

            builder.Property(x => x.Height)
                   .HasPrecision(5, 2)
                   .IsRequired();

            builder.Property(x => x.Weight)
                   .HasPrecision(5, 2)
                   .IsRequired();

            builder.Property(x => x.BloodType)
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(x => x.Note)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(500);

            builder.HasOne(x => x.Member)
                   .WithOne(x => x.HealthRecord)
                   .HasForeignKey<HealthRecord>(x => x.MemberId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
