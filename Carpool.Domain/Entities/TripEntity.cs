namespace Carpool.Domain.Entities
{
    public class Trip
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid DriverId { get; set; }
        public User Driver { get; set; }

        public Address DepartureAddress { get; set; }
        public Address DestinationAddress { get; set; }
        public DateTime DepartureTime { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsSmokingAllowed { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}