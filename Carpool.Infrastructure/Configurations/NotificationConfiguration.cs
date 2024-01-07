using Carpool.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carpool.Infrastructure.Data.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Message)
                .IsRequired();

            builder.Property(n => n.Timestamp)
                .IsRequired()
                .HasColumnType("timestamp without time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(n => n.Seen)
                .IsRequired()
                .HasDefaultValue(false);

            // Relationship with User
            builder.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship with Reservation
            builder.HasOne(n => n.Reservation)
                .WithMany()
                .HasForeignKey(n => n.ReservationId)
                .OnDelete(DeleteBehavior.SetNull);

            // Relationship with Trip
            builder.HasOne(n => n.Trip)
                .WithMany()
                .HasForeignKey(n => n.TripId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
