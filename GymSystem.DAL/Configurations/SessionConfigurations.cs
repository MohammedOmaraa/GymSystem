using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.DAL.Configurations
{
    internal class SessionConfigurations : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.Property(x => x.Description)
               .IsRequired()
               .HasMaxLength(500);

            builder.Property(x => x.Capacity)
                   .IsRequired();

            builder.Property(x => x.StartDate)
                   .IsRequired();

            builder.Property(x => x.EndDate)
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.UpdatedAt);

            builder.HasOne(x => x.Trainer)
                   .WithMany(t => t.Sessions)
                   .HasForeignKey(x => x.TrainerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Category)
                   .WithMany(c => c.Sessions)
                   .HasForeignKey(x => x.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_Session_Capacity",
                    "Capacity BETWEEN 1 AND 25");

                table.HasCheckConstraint(
                    "CK_Session_EndDateAfterStartDate",
                    "EndDate > StartDate");
            });


        }
    }
}
