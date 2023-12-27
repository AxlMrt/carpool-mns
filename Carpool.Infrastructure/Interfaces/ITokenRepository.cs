namespace Carpool.Infrastructure;

public interface ITokenRepository
{
    Task SaveTokenAsync(string userId, string token);
    Task<string> GetTokenByUserIdAsync(string userId);
    Task RemoveTokenAsync(string userId);
}
