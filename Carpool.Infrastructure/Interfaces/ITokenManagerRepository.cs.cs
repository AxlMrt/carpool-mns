using Carpool.Domain.Entities;

namespace Carpool.Infrastructure
{
    public interface ITokenManagerRepository
    {
        Task AddTokenAsync(Token token);
        Task<Token> GetTokenByUserIdAsync(Guid userId);
        Task RemoveTokenAsync(string token);
    }
}