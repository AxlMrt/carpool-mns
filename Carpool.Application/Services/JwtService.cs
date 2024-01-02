using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Carpool.Application;

public class JwtService : IJwtService
{
    private readonly string _secretKey;
    private readonly string _audience;
    private readonly string _issuer;

    public JwtService(string secretKey, string audience, string issuer)
    {
        _secretKey = secretKey;
        _audience = audience;
        _issuer = issuer;
    }

    public async Task<string> GenerateTokenAsync(string userId, string role)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(_secretKey);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", userId),
                new Claim(ClaimTypes.Role, role)
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            Audience = _audience,
            Issuer = _issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return await Task.FromResult(tokenHandler.WriteToken(token));
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                RequireExpirationTime = true,
                ValidateLifetime = true
            }, out SecurityToken validatedToken);

            return await Task.FromResult(true);
        }
        catch
        {
            return await Task.FromResult(false);
        }
    }

    public async Task<string> RefreshTokenAsync(string token)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(_secretKey);

        try
        {
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience
            }, out SecurityToken validatedToken);

            if (validatedToken is not JwtSecurityToken jwtToken || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            DateTime now = DateTime.UtcNow;
            double timeElapsed = (now - jwtToken.ValidFrom).TotalMinutes;
            bool shouldRefresh = timeElapsed >= 60 * 24; // Refresh if more than 24 hours have passed since token issue

            if (shouldRefresh)
            {
                string userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string userRole = principal.FindFirst(ClaimTypes.Role)?.Value;

                string newToken = await GenerateTokenAsync(userId, userRole);
                return newToken;
            }
        }
        catch (Exception)
        {
            throw;
        }

        return token;
    }
}
