namespace Carpool.Domain.DTO.User
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public ICollection<int> CarIds { get; set; }
        public ICollection<int> AddressIds { get; set; }
        public ICollection<int> ReservationIds { get; set; }
        public ICollection<int> FeedbackIds { get; set; }
    }

    public class CreateUserDTO
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class UpdateUserDTO
    {
        public int Id { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
