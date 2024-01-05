namespace Carpool.Application.DTO.Trip
{
    public class TripDTO
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public int CarId { get; set; }
        public int DepartureAddressId { get; set; }
        public int DestinationAddressId { get; set; }
        public DateTime DepartureTime { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsSmokingAllowed { get; set; }
    }

    public class CreateTripDTO
    {
        public int DriverId { get; set; }
        public int CarId { get; set; }
        public int DepartureAddressId { get; set; }
        public int DestinationAddressId { get; set; }
        public DateTime DepartureTime { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsSmokingAllowed { get; set; }
    }

    public class UpdateTripDTO
    {
        public int? DepartureAddressId { get; set; }
        public int? DestinationAddressId { get; set; }
        public DateTime DepartureTime { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsSmokingAllowed { get; set; }
    }
}