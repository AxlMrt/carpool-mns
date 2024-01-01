namespace Carpool.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid TripId { get; set; }
        public Trip Trip { get; set; }

        public int ReservedSeats { get; set; }
        public string Status { get; set; }
    }
}