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
        }

        public DbSet<User> Users { get; set; }
    }
}