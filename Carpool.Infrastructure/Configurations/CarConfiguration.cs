using Carpool.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carpool.Infrastructure.Configurations
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            // Primary key definition
            builder.HasKey(c => c.Id);

            // Required properties
            builder.Property(c => c.Brand).IsRequired();
            builder.Property(c => c.Model).IsRequired();

            // Relationship with Owner (User)
            builder.HasOne(c => c.Owner)
                   .WithMany(u => u.Cars)
                   .HasForeignKey(c => c.OwnerId)
                   .OnDelete(DeleteBehavior.Restrict); // Don't delete the car if the owner is deleted

            // Relationship with Trips
            builder.HasMany(c => c.Trips)
                   .WithOne(t => t.Car)
                   .HasForeignKey(t => t.CarId)
                   .OnDelete(DeleteBehavior.Restrict); // Don't delete the car if it's associated with a trip
        }
    }
}
