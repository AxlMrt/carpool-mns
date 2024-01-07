using Carpool.Domain.Entities;

public interface INotificationRepository
{
    Task<Notification> GetByIdAsync(int id);
    Task<IEnumerable<Notification>> GetAllAsync();
    Task AddAsync(Notification notification);
    Task UpdateAsync(Notification notification);
    Task DeleteAsync(Notification notification);
}