using Carpool.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carpool.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            // Required properties
            builder.Property(u => u.LastName).IsRequired();
            builder.Property(u => u.FirstName).IsRequired();
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.Password).IsRequired();
            builder.Property(u => u.Role).IsRequired();

            // Relationship User - Cars
            builder.HasMany(u => u.Cars)
                   .WithOne(c => c.Owner)
                   .HasForeignKey(c => c.OwnerId)
                   .OnDelete(DeleteBehavior.Restrict); // Delete associated cars if the user is deleted

            // Relationship User - Addresses
            builder.HasMany(u => u.Addresses)
                   .WithOne(a => a.User)
                   .HasForeignKey(a => a.UserId) 
                   .OnDelete(DeleteBehavior.Cascade); // Delete associated addresses if the user is deleted

            // Relationship User - Reservations
            builder.HasMany(u => u.Reservations)
                   .WithOne(r => r.User)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // Delete associated reservations if the user is deleted
            
            builder.HasMany(u => u.Notifications) // Ajout de la relation avec les notifications
                   .WithOne(n => n.User)
                   .HasForeignKey(n => n.UserId);
        }
    }
}
