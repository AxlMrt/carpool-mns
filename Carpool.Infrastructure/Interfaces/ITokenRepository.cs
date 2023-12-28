namespace Carpool.Infrastructure
{
    public interface ITokenRepository
    {
        Task SaveTokenAsync(string userId, string token);
        Task<string> GetTokenByUserMailAsync(string usermail);
        Task RemoveTokenAsync(string userMail);
    }
}