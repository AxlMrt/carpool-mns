namespace Carpool.Domain.Entities
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }

        public User? User { get; set; }
        public Trip? Trip { get; set; } 
        public Reservation Reservation { get; set; } 
    }
}