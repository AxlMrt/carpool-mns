using Carpool.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carpool.Infrastructure.Context
{
    public class CarpoolDbContext : DbContext
    {

        public CarpoolDbContext(DbContextOptions<CarpoolDbContext> options) : base(options)        
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Token>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Car>()
                .HasKey(c => c.Id);
            
            modelBuilder.Entity<Address>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Trip>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Reservation>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Feedback>()
                .HasKey(f => f.Id);

            // Configurations des relations

            // Relation entre User et Car (Un utilisateur peut avoir plusieurs voitures)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Cars)
                .WithOne(c => c.Owner)
                .HasForeignKey(c => c.OwnerId);

            // Relation entre User et Reservation (Un utilisateur peut effectuer plusieurs réservations)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Reservations)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId);

            // Relation entre Car et Trip (Une voiture peut être utilisée pour plusieurs trajets)
            modelBuilder.Entity<Car>()
                .HasMany(c => c.Trips)
                .WithOne(t => t.Car)
                .HasForeignKey(t => t.CarId);

            // Relation entre Trip et Reservation (Un trajet peut avoir plusieurs réservations)
            modelBuilder.Entity<Trip>()
                .HasMany(t => t.Reservations)
                .WithOne(r => r.Trip)
                .HasForeignKey(r => r.TripId);

            // Relation entre User et Feedback (Un utilisateur peut donner plusieurs retours d'expérience)
            modelBuilder.Entity<User>()
                .HasMany(u => u.FeedbacksGiven)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId);

            // Relation entre Trip et Feedback (Un trajet peut avoir plusieurs retours d'expérience)
            modelBuilder.Entity<Trip>()
                .HasMany(t => t.Feedbacks)
                .WithOne(f => f.Trip)
                .HasForeignKey(f => f.TripId);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
    }
}