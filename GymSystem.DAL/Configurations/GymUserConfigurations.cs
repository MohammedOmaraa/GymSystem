using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.DAL.Configurations
{
    internal class GymUserConfigurations<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name)
               .IsRequired()
               .HasColumnType("varchar")
               .HasMaxLength(50);

            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasColumnType("varchar")
                   .HasMaxLength(100);

            builder.Property(x => x.Phone)
                   .IsRequired()
                   .HasColumnType("varchar")
                   .HasMaxLength(11);

            builder.Property(x => x.DateOfBirth)
                   .IsRequired();

            builder.Property(x => x.Gender)
                   .HasConversion<int>()
                   .IsRequired();

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.BuildingNumber)
                       .HasColumnName("BuildingNumber")
                       .IsRequired();

                address.Property(a => a.Street)
                       .HasColumnName("Street")
                       .HasColumnType("varchar")
                       .HasMaxLength(30)
                       .IsRequired();

                address.Property(a => a.City)
                       .HasColumnName("City")
                       .HasColumnType("varchar")
                       .HasMaxLength(30)
                       .IsRequired();
            });

            builder.HasIndex(x => x.Email)
                   .IsUnique();

            builder.HasIndex(x => x.Phone)
                   .IsUnique();

            builder.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_GymUser_Email",
                    "Email LIKE '_%@_%._%'");

                table.HasCheckConstraint(
                    "CK_GymUser_Phone",
                    "Phone LIKE '010________' OR " +
                    "Phone LIKE '011________' OR " +
                    "Phone LIKE '012________' OR " +
                    "Phone LIKE '015________'");

                table.HasCheckConstraint(
                    "CK_GymUser_BuildingNumber",
                    "BuildingNumber > 0");
            });
        }
    }
}
