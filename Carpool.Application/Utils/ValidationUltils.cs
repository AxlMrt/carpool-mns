using System.Text.RegularExpressions;

namespace Carpool.Application.Utils
{
    public static class ValidationUtils
    {
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        }

        public static bool IsStrongPassword(string password)
        {
            // Minimal length 8char
            // At least 1 upper and lower case letter
            // At least one special char
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*]).{8,}$");
        }
    }
}