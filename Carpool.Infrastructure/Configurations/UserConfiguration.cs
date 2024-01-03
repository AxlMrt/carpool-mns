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

            builder.Property(u => u.LastName).IsRequired();
            builder.Property(u => u.FirstName).IsRequired();
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.Password).IsRequired();
            builder.Property(u => u.Role).IsRequired();

            builder.HasMany(u => u.Cars)
                   .WithOne(c => c.Owner)
                   .HasForeignKey(c => c.Owner)
                   .OnDelete(DeleteBehavior.Restrict); // Delete associated cars if the user is deleted

            builder.HasMany(u => u.Addresses)
                   .WithOne(a => a.User)
                   .HasForeignKey(a => a.User)
                   .OnDelete(DeleteBehavior.Cascade); // Delete associated adresses if the user is deleted

            builder.HasMany(u => u.Reservations)
                   .WithOne(r => r.User)
                   .HasForeignKey(r => r.User)
                   .OnDelete(DeleteBehavior.Cascade); // Delete associated reservation if the user is deleted

            builder.HasMany(u => u.FeedbacksGiven)
                   .WithOne(f => f.User)
                   .HasForeignKey(f => f.User)
                   .OnDelete(DeleteBehavior.Cascade); // Delete associated feedbacks if the user is deleted
        }
    }
}
