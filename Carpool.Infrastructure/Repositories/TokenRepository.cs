using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpool.Infrastructure
{
    public class TokenRepository : ITokenRepository
    {
        private readonly Dictionary<string, string> _tokens = new Dictionary<string, string>();

        public async Task SaveTokenAsync(string userMail, string token)
        {
            _tokens[userMail] = token;
            await Task.CompletedTask;
        }

        public async Task<string> GetTokenByUserMailAsync(string userMail)
        {
            return await Task.FromResult(_tokens.ContainsKey(userMail) ? _tokens[userMail] : null);
        }

        public async Task RemoveTokenAsync(string userId)
        {
            if (_tokens.ContainsKey(userId))
            {
                _tokens.Remove(userId);
            }
            await Task.CompletedTask;
        }
    }
}