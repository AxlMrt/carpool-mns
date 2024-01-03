namespace Carpool.Domain.DTOs
{
    public class FeedbackCreateDto
    {
        public string Comment { get; set; }
        public int Rating { get; set; }

        public int UserId { get; set; }
        public int? TripId { get; set; }
        public int? ReservationId { get; set; }
    }
}