
namespace Carpool.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool Seen { get; set; }
        public DateTime Timestamp { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int? ReservationId { get; set; }
        public Reservation Reservation { get; set; }

        public int? TripId { get; set; }
        public Trip Trip { get; set; }
    }
}
