using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Application.Utils;
using Carpool.Domain.DTO.Notifications;
using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;

namespace Carpool.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<NotificationDTO>> GetAllNotificationsAsync()
        {
            IEnumerable<Notification> notifications = await _notificationRepository.GetAllNotificationsAsync();

            if (notifications == null || !notifications.Any())
                throw new NotFoundException("No notifications found.");

            return notifications.Select(n => ObjectUpdater.MapObject<NotificationDTO>(n));
        }

        public async Task<NotificationDTO> GetNotificationByIdAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Notification notification = await _notificationRepository.GetNotificationByIdAsync(id) ?? throw new NotFoundException($"Notification with ID {id} not found.");

            return ObjectUpdater.MapObject<NotificationDTO>(notification);
        }

        public async Task<IEnumerable<NotificationDTO>> GetNotificationsByUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new BadRequestException("Invalid User ID.");

            IEnumerable<Notification> notifications = await _notificationRepository.GetNotificationsForUserAsync(userId);

            if (notifications == null || !notifications.Any())
                throw new NotFoundException($"No notifications found for user with ID {userId}.");

            return notifications.Select(n => ObjectUpdater.MapObject<NotificationDTO>(n));
        }

        public async Task<NotificationDTO> CreateNotificationAsync(NotificationCreationDTO notificationDto)
        {
            if (notificationDto == null)
                throw new BadRequestException("Notification object cannot be null.");

            Notification notification = ObjectUpdater.MapObject<Notification>(notificationDto);

            await _notificationRepository.AddNotificationAsync(notification);

            return ObjectUpdater.MapObject<NotificationDTO>(notification);
        }

        public async Task<NotificationDTO> UpdateNotificationAsync(int id, NotificationUpdateDTO notificationDto)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Notification existingNotification = await _notificationRepository.GetNotificationByIdAsync(id) ?? throw new NotFoundException($"Notification with ID {id} not found.");

            existingNotification.Content = notificationDto.Content;
            existingNotification.Seen = notificationDto.Seen;

            await _notificationRepository.UpdateNotificationAsync(existingNotification);

            return ObjectUpdater.MapObject<NotificationDTO>(existingNotification);
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Notification existingNotification = await _notificationRepository.GetNotificationByIdAsync(id) ?? throw new NotFoundException($"Notification with ID {id} not found.");

            await _notificationRepository.DeleteNotificationAsync(id);

            return true;
        }
    }
}
