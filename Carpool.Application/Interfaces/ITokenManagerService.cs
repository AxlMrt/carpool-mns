using Carpool.Domain.Entities;

namespace Carpool.Application.Services
{
    public interface ITokenManagerService
    {
        Task<Token> AddTokenAsync(User user, string token);
        Task UpdateTokenAsync(string oldToken, string newToken);
        Task RemoveTokenAsync(string token);
    }
}