namespace Carpool.Infrastructure;

public class TokenRepository : ITokenRepository
{
    private readonly Dictionary<string, string> _tokens = new Dictionary<string, string>();

    public async Task SaveTokenAsync(string userId, string token)
    {
        _tokens[userId] = token;
        await Task.CompletedTask;
    }

    public async Task<string> GetTokenByUserIdAsync(string userId)
    {
        return await Task.FromResult(_tokens.ContainsKey(userId) ? _tokens[userId] : null);
    }

    public async Task RemoveTokenAsync(string userId)
    {
        if (_tokens.ContainsKey(userId))
            _tokens.Remove(userId);
        
        await Task.CompletedTask;
    }
}
