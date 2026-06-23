using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.DAL.Configurations
{
    internal class SessionConfigurations : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(T =>
            {
                T.HasCheckConstraint("CK_Session_Capacity", "Capacity BETWEEN 1 AND 25");
                T.HasCheckConstraint("CK_Session_EndDateAfterStartDate", "EndDate > StartDate");
            });

            builder.HasOne(X => X.Trainer)
                .WithMany(X => X.Sessions)
                .HasForeignKey(X => X.TrainerId);

            builder.HasOne(X => X.Category)
                .WithMany(X => X.Sessions)
                .HasForeignKey(X => X.CategoryId);


        }
    }
}
