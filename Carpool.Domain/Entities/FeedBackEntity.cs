namespace Carpool.Domain.Entities
{
    public class Feedback
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Comment { get; set; }
        public int Rating { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid? TripId { get; set; } 
        public Trip Trip { get; set; } 

        public Guid? ReservationId { get; set; }
        public Reservation Reservation { get; set; } 
    }
}