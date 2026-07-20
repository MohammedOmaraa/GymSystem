using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.DAL.Configurations
{
    internal class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.CategoryName)
               .IsRequired()
               .HasColumnType("varchar")
               .HasMaxLength(30);

            builder.HasIndex(x => x.CategoryName)
                   .IsUnique();

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.UpdatedAt);

            builder.HasData(
                             new Category { Id = 1, CategoryName = "Cardio" },
                             new Category { Id = 2, CategoryName = "Strength" },
                             new Category { Id = 3, CategoryName = "Yoga" },
                             new Category { Id = 4, CategoryName = "Boxing" },
                             new Category { Id = 5, CategoryName = "CrossFit" }
                         );

        }
    }
}
