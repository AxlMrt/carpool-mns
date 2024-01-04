namespace Carpool.Domain.DTOs
{
    public class FeedbackUpdateDto
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public int? Rating { get; set; }
    }
}