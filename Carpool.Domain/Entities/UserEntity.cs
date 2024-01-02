namespace Carpool.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = Roles.Roles.User;

        public ICollection<Car> Cars { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Feedback> FeedbacksGiven { get; set; }
    }
}