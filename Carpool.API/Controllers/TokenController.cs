using Carpool.Application;
using Carpool.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Carpool.API;

public class TokenController : ControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly ITokenRepository _tokenRepository;

    public TokenController(IJwtService jwtService, ITokenRepository tokenRepository)
    {
        _jwtService = jwtService;
        _tokenRepository = tokenRepository;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTokenByUserId(string id)
    {
        var token = await _tokenRepository.GetTokenByUserIdAsync(id);
        if (token != null)
            return Ok(new { Token = token });
        
        return NotFound(new { Message = "No token found for this user." });
    }

    [HttpPost]
    public async Task<IActionResult> GenerateToken(string userId)
    {
        var token = await _jwtService.GenerateTokenAsync(userId);
        await _tokenRepository.SaveTokenAsync(userId, token);

        return Ok(new { Token = token });
    }

    [HttpPost("validateToken")]
    public async Task<IActionResult> ValidateToken(string token)
    {
        var isValid = await _jwtService.ValidateTokenAsync(token);
        if (isValid)
            return Ok(new { Valid = true, Message = "Token is valid. "});
        
        return BadRequest(new { Valid = false, Message = "Token isn't valid." });
    }

    [HttpPost("revokeToken")]
    public async Task<IActionResult> RevokeToken(string userId)
    {
        await _tokenRepository.RemoveTokenAsync(userId);
        return Ok(new { Message = "Token has been removed." });
    }
}
