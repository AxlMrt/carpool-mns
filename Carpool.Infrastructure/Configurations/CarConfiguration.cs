using Carpool.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carpool.Infrastructure.Configurations
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Brand).IsRequired();
            builder.Property(c => c.Model).IsRequired();
            // Other potential properties...

            builder.HasOne(c => c.Owner)
                   .WithMany(u => u.Cars)
                   .HasForeignKey(c => c.Owner)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasMany(c => c.Trips)
                   .WithOne(t => t.Car)
                   .HasForeignKey(t => t.Car)
                   .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
