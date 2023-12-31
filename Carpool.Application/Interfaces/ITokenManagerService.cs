using Carpool.Domain.Entities;

namespace Carpool.Application.Services
{
    public interface ITokenManagerService
    {
        Task<Token> AddTokenAsync(string userId, string token);
        Task UpdateTokenAsync(string oldToken, string newToken);
        Task RemoveTokenAsync(string token);
    }
}