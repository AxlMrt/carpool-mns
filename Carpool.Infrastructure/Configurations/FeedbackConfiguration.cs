using Carpool.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carpool.Infrastructure.Configurations
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Comment).IsRequired();
            builder.Property(f => f.Rating).IsRequired();

            builder.HasOne(f => f.User)
                   .WithMany(u => u.FeedbacksGiven)
                   .HasForeignKey(f => f.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // Delete associated feedback if the user is deleted

            // Optional relationships for Trip 
            builder.HasOne(f => f.Trip)
                   .WithMany(t => t.Feedbacks)
                   .HasForeignKey(f => f.TripId)
                   .OnDelete(DeleteBehavior.Cascade); // Delete associated feedback if the trip is deleted
        }
    }
}
