using System.Security.Claims;

namespace Carpool.Application;

public interface IJwtService
{
    Task<string> GenerateToken(string userId, string userEmail);
    Task<ClaimsPrincipal> GetPrincipalFromTokenAsync(string token);

}
