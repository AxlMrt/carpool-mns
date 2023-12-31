using Carpool.Domain.DTOs;
using Carpool.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Carpool.Domain;
using Carpool.Domain.Entities;

namespace Carpool.API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid registration data.");

                await _authService.RegisterUserAsync(user);

                return Ok("User registered successfully.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while registering the user.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto loginData)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid login data.");
                
                Token token = await _authService.AuthenticateAsync(loginData);
    
                if (token is null)
                    return Unauthorized("Invalid username or password.");
                
                return Ok(new { AccessToken = token.TokenString });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

                await _authService.LogoutAsync(token);

                return Ok("Logged out successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var newToken = await _authService.RefreshTokenAsync(token);

                return Ok(new { AccessToken = newToken });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}