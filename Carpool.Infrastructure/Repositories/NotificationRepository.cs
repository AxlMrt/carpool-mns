using Carpool.Domain.Entities;
using Carpool.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Carpool.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly CarpoolDbContext _dbContext;

        public NotificationRepository(CarpoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Notification> GetByIdAsync(int id)
        {
            return await _dbContext.Notifications.FindAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _dbContext.Notifications.ToListAsync();
        }

        public async Task AddAsync(Notification notification)
        {
            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Notification notification)
        {
            _dbContext.Notifications.Update(notification);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Notification notification)
        {
            _dbContext.Notifications.Remove(notification);
            await _dbContext.SaveChangesAsync();
        }
    }
}