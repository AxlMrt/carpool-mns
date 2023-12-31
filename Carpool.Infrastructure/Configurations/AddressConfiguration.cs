using Carpool.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carpool.Infrastructure.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            // Primary key definition
            builder.HasKey(a => a.Id);

            // Required properties
            builder.Property(a => a.Street).IsRequired();
            builder.Property(a => a.City).IsRequired();
            builder.Property(a => a.PostalCode).IsRequired();
            builder.Property(a => a.Country).IsRequired();

            // Relationship with User
            builder.HasOne(a => a.User)
                   .WithMany(u => u.Addresses)
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // Adjust deletion behavior as needed
        }
    }
}
