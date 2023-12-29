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
        private readonly IPasswordHasherService _passwordHasherService;
        public AuthService(IAuthRepository authRepository, IPasswordHasherService passwordHasherService, IJwtService jwtService)
        {
            _authRepository = authRepository;
            _passwordHasherService = passwordHasherService;
        }

        public async Task RegisterUserAsync(RegisterUserDto user)
        {
            user.Password = _passwordHasherService.HashPassword(user.Password);
            await _authRepository.RegisterUserAsync(user);
        }

        public async Task<User> AuthenticateAsync(LoginDto loginData)
        {
            User user = await _authRepository.FindUserAsync(loginData.Email);

            if (user is null || !_passwordHasherService.VerifyPassword(user.Password, loginData.Password))
                return null;

            return user;
        }
    }
}