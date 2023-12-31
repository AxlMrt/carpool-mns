namespace Carpool.Domain.Entities
{
    public class Token
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string TokenString { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}