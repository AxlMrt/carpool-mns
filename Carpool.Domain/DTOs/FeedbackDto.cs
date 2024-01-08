using Carpool.Domain.Entities;

namespace Carpool.Domain.DTOs.Feedback
{
    public class FeedbackDTO
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
    public class CreateFeedbackDTO
    {
        public string Comment { get; set; }
        public int Rating { get; set; }
        public int UserId { get; set; }
        public int TripId { get; set; }
    }

    public class UpdateFeedbackDTO
    {
        public string? Comment { get; set; }
        public int? Rating { get; set; }
    }
}