namespace Carpool.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        public User User { get; set; }
        public Trip Trip { get; set; }

        public int ReservedSeats { get; set; }
        public string Status { get; set; }
    }
}