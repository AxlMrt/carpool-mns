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
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
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

        var token = tokenHandler.CreateToken(tokenDescriptor);
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
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
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

            var jwtToken = validatedToken as JwtSecurityToken;
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            var now = DateTime.UtcNow;
            var timeElapsed = (now - jwtToken.ValidFrom).TotalMinutes;
            var shouldRefresh = timeElapsed >= 60 * 24; // Refresh if more than 24 hours have passed since token issue

            if (shouldRefresh)
            {
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = principal.FindFirst(ClaimTypes.Role)?.Value;

                var newToken = await GenerateTokenAsync(userId, userRole);
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
