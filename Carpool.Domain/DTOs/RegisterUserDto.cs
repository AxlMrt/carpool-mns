namespace Carpool.API.DTOs
{
    public class RegisterUserDto
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}