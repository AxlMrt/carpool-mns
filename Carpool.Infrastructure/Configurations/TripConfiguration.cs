using Carpool.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carpool.Infrastructure.Configurations
{
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            // Primary key definition
            builder.HasKey(t => t.Id);

            // Required properties
            builder.Property(t => t.DepartureTime).IsRequired();
            builder.Property(t => t.AvailableSeats).IsRequired();
            builder.Property(t => t.IsSmokingAllowed).IsRequired();

            // Relationship with User (Driver)
            builder.HasOne(t => t.Driver)
                   .WithMany()
                   .HasForeignKey(t => t.Driver) // Assuming DriverId is the foreign key property
                   .OnDelete(DeleteBehavior.Restrict); // Prevent deletion if the associated driver is deleted

            // Relationship with Car
            builder.HasOne(t => t.Car)
                   .WithMany(c => c.Trips)
                   .HasForeignKey(t => t.Car) // Assuming CarId is the foreign key property
                   .OnDelete(DeleteBehavior.Restrict); // Prevent deletion if the associated car is deleted

            // Relationship with Departure Address
            builder.HasOne(t => t.DepartureAddress)
                   .WithMany()
                   .HasForeignKey(t => t.DepartureAddressId)
                   .OnDelete(DeleteBehavior.Restrict); // Prevent deletion if the associated departure address is deleted

            // Relationship with Destination Address
            builder.HasOne(t => t.DestinationAddress)
                   .WithMany()
                   .HasForeignKey(t => t.DestinationAddressId)
                   .OnDelete(DeleteBehavior.Restrict); // Prevent deletion if the associated destination address is deleted

            // Relationship with Reservations
            builder.HasMany(t => t.Reservations)
                   .WithOne(r => r.Trip)
                   .HasForeignKey(r => r.Trip) // Assuming TripId is the foreign key property in Reservation
                   .OnDelete(DeleteBehavior.Cascade); // Delete associated reservations if the trip is deleted

            // Relationship with Feedbacks
            builder.HasMany(t => t.Feedbacks)
                   .WithOne(f => f.Trip)
                   .HasForeignKey(f => f.Trip) // Assuming TripId is the foreign key property in Feedback
                   .OnDelete(DeleteBehavior.Cascade); // Delete associated feedback if the trip is deleted
        }
    }
}
