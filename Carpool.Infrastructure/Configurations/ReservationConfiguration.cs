using Carpool.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carpool.Infrastructure.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.ReservedSeats).IsRequired();
            builder.Property(r => r.Status).IsRequired();

            builder.HasOne(r => r.User)
                   .WithMany(u => u.Reservations)
                   .HasForeignKey(r => r.UserId) 
                   .OnDelete(DeleteBehavior.Cascade); // Delete associated reservations if the user is deleted

            builder.HasOne(r => r.Trip)
                   .WithMany(t => t.Reservations)
                   .HasForeignKey(r => r.TripId)
                   .OnDelete(DeleteBehavior.Cascade); // Delete associated reservations if the trip is deleted
        }
    }
}
