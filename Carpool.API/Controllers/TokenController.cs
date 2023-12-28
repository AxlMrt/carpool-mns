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

    [HttpPost]
    public async Task<IActionResult> GenerateTokenAsync(string userMail)
    {
        var token = await _jwtService.GenerateTokenAsync(userMail);
        await _tokenRepository.SaveTokenAsync(userMail, token);
        return Ok(new { Token = token });
    }

    [HttpPost("validate")]
    public async Task<IActionResult> ValidateTokenAsync(string token)
    {
        var isValid = await _jwtService.ValidateTokenAsync(token);
        if (isValid)
        {
            return Ok(new { Valid = true, Message = "Token is valid." });
        }
        else
        {
            return BadRequest(new { Valid = false, Message = "Token is invalid." });
        }
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeTokenAsync(string userMail)
    {
        await _tokenRepository.RemoveTokenAsync(userMail);
        return Ok(new { Message = "Token has been removed successfully" });
    }

    [HttpGet("{userMail}")]
    public async Task<IActionResult> GetTokenByUserIdAsync(string userMail)
    {
        var token = await _tokenRepository.GetTokenByUserMailAsync(userMail);
        if (token != null)
        {
            return Ok(new { Token = token });
        }
        else
        {
            return NotFound(new { Message = "No token found for this user." });
        }
    }

}
