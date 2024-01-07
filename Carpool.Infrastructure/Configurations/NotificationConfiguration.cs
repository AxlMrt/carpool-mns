using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Carpool.Domain.Entities;

namespace Carpool.Infrastructure.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Content).HasColumnName("Content").IsRequired();
            builder.Property(n => n.Seen).HasColumnName("Seen").IsRequired();
            builder.Property(n => n.Timestamp).HasColumnName("Timestamp").IsRequired();

            builder.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .IsRequired();

            builder.HasOne(n => n.Reservation)
                .WithMany()
                .HasForeignKey(n => n.ReservationId);

            builder.HasOne(n => n.Trip)
                .WithMany()
                .HasForeignKey(n => n.TripId);

            builder.HasOne(n => n.Message)
                .WithMany()
                .HasForeignKey(n => n.MessageId);

            builder.HasOne(n => n.Feedback)
                .WithMany()
                .HasForeignKey(n => n.FeedbackId);
        }
    }
}
