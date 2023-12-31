using Carpool.Domain.DTOs;
using Carpool.Application.Interfaces;
using Carpool.Infrastructure.Interfaces;
using Carpool.Domain;
using Carpool.Domain.Entities;

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

        public async Task RegisterUserAsync(RegisterUserDto user)
        {
            user.Password = _passwordHasherService.HashPassword(user.Password);
            await _authRepository.RegisterUserAsync(user);
        }

        public async Task<Token> AuthenticateAsync(LoginDto loginData)
        {
            User user = await _authRepository.FindUserAsync(loginData.Email);

            if (user is null || !_passwordHasherService.VerifyPassword(user.Password, loginData.Password))
                return null;

            var token = await _jwtService.GenerateTokenAsync(user.Id.ToString(), user.Role);

            return await _tokenManagerService.AddTokenAsync(user.Id.ToString(), token);
        }

        public async Task LogoutAsync(string token)
        {
            await _tokenManagerService.RemoveTokenAsync(token);
        }
    }
}