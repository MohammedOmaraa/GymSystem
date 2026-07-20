using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.DAL.Configurations
{
    internal class BookingConfigurations : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Property(x => x.BookingDate)
                   .HasDefaultValueSql("GETUTCDATE()")
                   .IsRequired();

            builder.Property(x => x.IsAttended)
                   .HasDefaultValue(false)
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.UpdatedAt);

            builder.HasIndex(x => new
            {
                x.MemberId,
                x.SessionId
            })
            .IsUnique();

            builder.HasOne(x => x.Member)
                   .WithMany(x => x.Bookings)
                   .HasForeignKey(x => x.MemberId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Session)
                   .WithMany(x => x.Bookings)
                   .HasForeignKey(x => x.SessionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
