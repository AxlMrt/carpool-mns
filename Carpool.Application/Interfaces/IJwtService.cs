namespace Carpool.Application;

public interface IJwtService
{
    Task<string> GenerateTokenAsync(string userId, string role);
    Task<bool> ValidateTokenAsync(string token);
}
