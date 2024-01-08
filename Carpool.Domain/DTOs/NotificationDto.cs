using Carpool.Domain.Common;

namespace Carpool.Domain.DTO.Notifications
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool Seen { get; set; }
        public DateTime Timestamp { get; set; }
        public NotificationType Type { get; set; }
    }

    public class NotificationCreationDTO
    {
        public string Content { get; set; }
        public bool Seen { get; set; }
        public DateTime Timestamp { get; set; }
        public NotificationType Type { get; set; }
        public int UserId { get; set; }
        public int? ReservationId { get; set; }
        public int? TripId { get; set; }
        public int? MessageId { get; set; }
        public int? FeedbackId { get; set; }
    }

    public class NotificationUpdateDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool Seen { get; set; }
        public NotificationType Type { get; set; }
    }
}