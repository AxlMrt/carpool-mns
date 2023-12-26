using System.Security.Claims;

namespace Carpool.Application;

public interface IJwtService
{
    Task<string> GenerateTokenAsync(string userId, string userEmail);
    Task<bool> ValidateTokenAsync(string token);

}
