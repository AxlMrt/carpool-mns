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
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}