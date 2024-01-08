using Carpool.Domain.Common;

namespace Carpool.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }

        public int ReservedSeats { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    }
}