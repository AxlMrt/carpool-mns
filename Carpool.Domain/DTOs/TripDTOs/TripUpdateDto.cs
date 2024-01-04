using Carpool.Domain.Entities;

namespace Carpool.Domain.DTOs
{
    public class TripUpdateDto
    {
        public int Id { get; set; }

        public int CarId { get; set; }

        public int? DepartureAddressId { get; set; }

        public int? DestinationAddressId { get; set; }

        public DateTime DepartureTime { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsSmokingAllowed { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}