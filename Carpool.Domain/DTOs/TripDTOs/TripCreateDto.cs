namespace Carpool.Domain.DTOs
{
    public class TripCreateDto
    {
        public int DriverId { get; set; }

        public int CarId { get; set; }

        public int DepartureAddressId { get; set; }

        public int DestinationAddressId { get; set; }

        public DateTime DepartureTime { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsSmokingAllowed { get; set; }
    }
}