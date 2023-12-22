using Carpool.Domain.DTOs;
using Carpool.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Carpool.API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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

                await _authService.RegisterUserAsync(user);

                return Ok("User registered successfully.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while registering the user.");
            }
        }


    }
}