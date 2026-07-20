using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.DAL.Configurations
{
    internal class TrainerConfigurations : GymUserConfigurations<Trainer>, IEntityTypeConfiguration<Trainer>
    {
        public new void Configure(EntityTypeBuilder<Trainer> builder)
        {
            base.Configure(builder);

            builder.ToTable("Trainers", table =>
            {
                table.HasCheckConstraint(
                    "CK_Trainer_HireDate",
                    "HireDate <= GETDATE()");
            });

            builder.Property(x => x.Specialty)
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(x => x.HireDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasMany(x => x.Sessions)
                   .WithOne(x => x.Trainer)
                   .HasForeignKey(x => x.TrainerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
