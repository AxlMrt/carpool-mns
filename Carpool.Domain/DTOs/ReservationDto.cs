using Carpool.Domain.Entities;

namespace Carpool.Domain.DTO.Reservation
{
    public class ReservationDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TripId { get; set; }
        public int ReservedSeats { get; set; }
        public ReservationStatus Status { get; set; }
    }

    public class CreateReservationDTO
    {
        public int UserId { get; set; }
        public int TripId { get; set; }
        public int ReservedSeats { get; set; }
    }

    public class UpdateReservationDTO
    {
        public int Id { get; set; }
        public int ReservedSeats { get; set; }
        public ReservationStatus Status { get; set; }
    }
}
