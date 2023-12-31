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

        public async Task AddTokenAsync(string userId, string token)
        {
            var expiryDate = DateTime.UtcNow.AddDays(1);

            var newToken = new Token
            {
                UserId = userId,
                TokenString = token,
                ExpiryDate = expiryDate
            };

            await _tokenManagerRepository.AddTokenAsync(newToken);
        }

        public async Task RemoveTokenAsync(string token)
        {
            await _tokenManagerRepository.RemoveTokenAsync(token);
        }
    }
}