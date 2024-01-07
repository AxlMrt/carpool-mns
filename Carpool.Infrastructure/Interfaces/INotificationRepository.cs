using Carpool.Domain.Entities;

namespace Carpool.Domain.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        Task<Notification> GetNotificationByIdAsync(int notificationId);
        Task<IEnumerable<Notification>> GetNotificationsForUserAsync(int userId);
        Task<IEnumerable<Notification>> GetUnseenNotificationsForUserAsync(int userId);
        Task AddNotificationAsync(Notification notification);
        Task UpdateNotificationAsync(Notification notification);
        Task DeleteNotificationAsync(int notificationId);
    }
}
