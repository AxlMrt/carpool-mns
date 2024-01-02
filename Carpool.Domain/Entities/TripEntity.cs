namespace Carpool.Domain.Entities
{
    public class Trip
    {
        public int Id { get; set; }

        public User Driver { get; set; }
        public Car Car { get; set; }

        public int? DepartureAddressId { get; set; }
        public Address DepartureAddress { get; set; }

        public int? DestinationAddressId { get; set; }
        public Address DestinationAddress { get; set; }

        public DateTime DepartureTime { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsSmokingAllowed { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}