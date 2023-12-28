namespace Carpool.Application;

public interface IJwtService
{
    Task<string> GenerateTokenAsync(string userId);
    Task<bool> ValidateTokenAsync(string token);
}
