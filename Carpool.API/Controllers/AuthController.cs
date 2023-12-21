using Carpool.API.DTOs;
using Carpool.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Carpool.API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;
        private readonly IPasswordHasherService _passwordHasherService;

        public AuthController(IAuthService authService, IPasswordHasherService passwordHasherService)
        {
            _authService = authService;
            _passwordHasherService = passwordHasherService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid registration data.");
                }

                user.Password = _passwordHasherService.HashPassword(user.Password);

                await _authService.RegisterUserAsync(user);

                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while registering the user.");
            }
        }


    }
}