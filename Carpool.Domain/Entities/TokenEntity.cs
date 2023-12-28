namespace Carpool.Domain.Entities
{
    public class Token
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserMail { get; set; }
        public string TokenValue { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}