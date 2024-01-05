namespace Carpool.Domain.DTOs
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }
        
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        
        public int RecipientId { get; set; }
        public string RecipientName { get; set; }
        
        public int TripId { get; set; }
        public string TripTitle { get; set; }
    }
}
