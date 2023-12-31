using Carpool.Domain.Entities;

namespace Carpool.Application.Services
{
    public interface ITokenManagerService
    {
        Task<Token> AddTokenAsync(string userId, string token);
        Task RemoveTokenAsync(string token);
    }
}