namespace Carpool.Application.DTO.Auth
{
    public class RegisterUserDTO
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
