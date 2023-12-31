using Carpool.Domain.Entities;

namespace Carpool.Infrastructure
{
    public interface ITokenManagerRepository
    {
        Task AddTokenAsync(Token token);
        Task<Token> GetTokenByUserIdAsync(string userId);
        Task RemoveTokenAsync(string token);
    }
}