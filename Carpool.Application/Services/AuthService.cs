using Carpool.Application.DTO.Auth;
using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Application.Utils;
using Carpool.Domain.Entities;
using Carpool.Infrastructure.Interfaces;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Carpool.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenManagerService _tokenManagerService;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IJwtService _jwtService;

        public AuthService(IAuthRepository authRepository, IPasswordHasherService passwordHasherService, IJwtService jwtService, ITokenManagerService tokenManagerService)
        {
            _authRepository = authRepository;
            _passwordHasherService = passwordHasherService;
            _jwtService = jwtService;
            _tokenManagerService = tokenManagerService;
        }

        public async Task RegisterUserAsync(RegisterUserDTO userDto)
        {
            if (userDto is null)
                throw new BadRequestException("User object cannot be null");

            if (!ValidationUtils.IsValidEmail(userDto.Email))
                throw new BadRequestException("Invalid email format");

            if (!ValidationUtils.IsStrongPassword(userDto.Password))
                throw new BadRequestException("Password should be at least 8 characters long, contain at least one uppercase, one lowercase, and one special character.");

            User user = await _authRepository.FindUserAsync(userDto.Email);

            if (user != null)
                throw new BadRequestException("This email is already associated with an account.");

            userDto.Password = _passwordHasherService.HashPassword(userDto.Password);
    
            User newUser = ObjectUpdater.MapObject<User>(userDto);
            await _authRepository.RegisterUserAsync(newUser);
        }

        public async Task<Token> AuthenticateAsync(LoginDTO loginData)
        {
            User user = await _authRepository.FindUserAsync(loginData.Email) ?? throw new InvalidCredentialException("Invalid email.");

            if (!_passwordHasherService.VerifyPassword(user.Password, loginData.Password))
                throw new InvalidCredentialException("Invalid password.");

            string token = await _jwtService.GenerateTokenAsync(user.Id.ToString(), user.Role) ?? throw new JwtGenerationException("Failed to generate JWT token.");

            return await _tokenManagerService.AddTokenAsync(user, token);
        }

        public async Task LogoutAsync(string token)
        {
            await _tokenManagerService.RemoveTokenAsync(token);
        }

        public async Task<string> RefreshTokenAsync(string token)
        {
            string newToken = await _jwtService.RefreshTokenAsync(token) ?? throw new TokenOperationException("Failed to refresh token.");
            await _tokenManagerService.UpdateTokenAsync(token, newToken);

            return newToken;
        }
    }
}
