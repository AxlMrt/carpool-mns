namespace Carpool.Domain.Entities
{
    public enum NotificationType
    {
        ReservationUpdate,
        NewReservation,
        MessageReceived,
        FeedbackCreated,
        FeedbackUpdated,
    }

    public class Notification
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool Seen { get; set; }
        public DateTime Timestamp { get; set; }
        public NotificationType Type { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int? ReservationId { get; set; }
        public Reservation Reservation { get; set; }

        public int? TripId { get; set; }
        public Trip Trip { get; set; }

        public int? MessageId { get; set; }
        public Message Message { get; set; }

        public int? FeedbackId { get; set; }
        public Feedback Feedback { get; set; }
    }
}
