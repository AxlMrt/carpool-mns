using System.Collections.Generic;
using System.Threading.Tasks;
using Carpool.Domain.Entities;
using Carpool.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Carpool.Infrastructure
{
    public class TokenRepository : ITokenRepository
    {
        private readonly CarpoolDbContext _dbContext;

        public TokenRepository(CarpoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveTokenAsync(string userMail, string token)
        {
            Token newToken = new Token
            {
                UserMail = userMail,
                TokenValue = token,
                ExpiryDate = DateTime.UtcNow.AddDays(1) // Expiration in 1 day
            };

            _dbContext.Tokens.Add(newToken);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> GetTokenByUserMailAsync(string userMail)
        {
            Token token = await _dbContext.Tokens.FirstOrDefaultAsync(t => t.UserMail == userMail && t.ExpiryDate > DateTime.UtcNow);
            return token.TokenValue;
        }

        public async Task RemoveTokenAsync(string userMail)
        {
            Token token = await _dbContext.Tokens.FirstOrDefaultAsync(t => t.UserMail == userMail);
            if (token != null)
            {
                _dbContext.Tokens.Remove(token);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}