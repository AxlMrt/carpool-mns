using Carpool.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Carpool.Domain.Entities;
using Carpool.Application.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Carpool.Application.DTO.Auth;

namespace Carpool.API.Controllers
{
    public class AuthController : BaseApiController, IExceptionFilter
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserDTO user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid registration data.");

            await _authService.RegisterUserAsync(user);

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(LoginDTO loginData)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid login data.");

            Token token = await _authService.AuthenticateAsync(loginData);

            if (token is null)
                return Unauthorized("Invalid username or password.");

            return Ok(new { AccessToken = token.TokenString });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            string token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            await _authService.LogoutAsync(token);

            return Ok("Logged out successfully");
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            string token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            string newToken = await _authService.RefreshTokenAsync(token);

            return Ok(new { AccessToken = newToken });
        }

        [NonAction]
        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
            int statusCode = 500;

            if (exception is NotFoundException)
                statusCode = 404;
            else if (exception is BadRequestException || exception is ArgumentException)
                statusCode = 400;
            else if (exception is JwtGenerationException || exception is TokenOperationException)
                statusCode = 500;

            context.Result = new ObjectResult(exception.Message)
            {
                StatusCode = statusCode
            };
        }
    }
}
