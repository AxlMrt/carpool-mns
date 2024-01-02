using Carpool.Domain.Entities;
using Carpool.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Carpool.Infrastructure.Repositories
{
    public class TokenManagerRepository : ITokenManagerRepository
    {
        private readonly CarpoolDbContext _context;

        public TokenManagerRepository(CarpoolDbContext context)
        {
            _context = context;
        }

        public async Task AddTokenAsync(Token token)
        {
            _context.Tokens.Add(token);
            await _context.SaveChangesAsync();
        }

        public async Task<Token> GetTokenByUserIdAsync(int id)
        {
            return await _context.Tokens.FirstOrDefaultAsync(t => t.User.Id == id);
        }

        public async Task UpdateTokenAsync(string oldTokenString, string newTokenString)
        {
            Token tokenEntity = await _context.Tokens.FirstOrDefaultAsync(t => t.TokenString == oldTokenString);
            if (tokenEntity != null)
            {
                tokenEntity.TokenString = newTokenString;
                tokenEntity.ExpiryDate = DateTime.UtcNow.AddDays(1);

                _context.Tokens.Update(tokenEntity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveTokenAsync(string token)
        {
            var tokenEntity = await _context.Tokens.FirstOrDefaultAsync(t => t.TokenString == token);
            if (tokenEntity != null)
            {
                _context.Tokens.Remove(tokenEntity);
                await _context.SaveChangesAsync();
            }
        }
    }
}