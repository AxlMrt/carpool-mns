using Carpool.Domain.DTO.Notifications;

namespace Carpool.Application.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDTO>> GetAllNotificationsAsync();
        Task<NotificationDTO> GetNotificationByIdAsync(int id);
        Task<IEnumerable<NotificationDTO>> GetNotificationsByUserIdAsync(int userId);
        Task<NotificationDTO> CreateNotificationAsync(NotificationCreationDTO notificationDto);
        Task<NotificationDTO> UpdateNotificationAsync(int id, NotificationUpdateDTO notificationDto);
        Task<bool> DeleteNotificationAsync(int id);
    }
}