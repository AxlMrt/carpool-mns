namespace Carpool.Domain.Entities
{
    public class Token
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string TokenString { get; set; }
        public DateTime ExpiryDate { get; set; }

    }
}