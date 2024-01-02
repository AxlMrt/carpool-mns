using Carpool.Domain.Entities;
using Carpool.Infrastructure;

namespace Carpool.Application.Services
{
    public class TokenManagerService : ITokenManagerService
    {
        private readonly ITokenManagerRepository _tokenManagerRepository;

        public TokenManagerService(ITokenManagerRepository tokenManagerRepository)
        {
            _tokenManagerRepository = tokenManagerRepository;
        }

        public async Task<Token> AddTokenAsync(User user, string token)
        {
            DateTime expiryDate = DateTime.UtcNow.AddDays(1);

            Token newToken = new()
            {
                User = user,
                TokenString = token,
                ExpiryDate = expiryDate
            };

            await _tokenManagerRepository.AddTokenAsync(newToken);
            return newToken;
        }

        public async Task UpdateTokenAsync(string oldToken, string newToken)
        {
            await _tokenManagerRepository.UpdateTokenAsync(oldToken, newToken);
        }

        public async Task RemoveTokenAsync(string token)
        {
            await _tokenManagerRepository.RemoveTokenAsync(token);
        }
    }
}