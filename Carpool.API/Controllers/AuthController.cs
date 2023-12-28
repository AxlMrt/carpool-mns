using Carpool.Domain.DTOs;
using Carpool.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Carpool.Domain;
using Carpool.Application;

namespace Carpool.API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;

        public AuthController(IAuthService authService, IJwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
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
        public async Task<IActionResult> LoginUser([FromBody] LoginDto loginData)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid login data.");
                
                bool isAuthenticated = await _authService.AuthenticateAsync(loginData);
    
                if (!isAuthenticated)
                    return Unauthorized("Invalid username or password.");
                
                var token = _jwtService.GenerateTokenAsync(loginData.Email);
                
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}