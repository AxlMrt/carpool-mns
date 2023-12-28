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
        private readonly IJwtService _jwtService;
        public AuthService(IAuthRepository authRepository, IPasswordHasherService passwordHasherService, IJwtService jwtService)
        {
            _authRepository = authRepository;
            _passwordHasherService = passwordHasherService;
            _jwtService = jwtService;
        }

        public async Task RegisterUserAsync(RegisterUserDto user)
        {
            user.Password = _passwordHasherService.HashPassword(user.Password);
            await _authRepository.RegisterUserAsync(user);
        }

        public async Task<bool> AuthenticateAsync(LoginDto loginData)
        {
            User user = await _authRepository.FindUserAsync(loginData.Email);
    
            if (user is null)
                return await Task.FromResult(false);
            
            var isAuthenticated = _passwordHasherService.VerifyPassword(user.Password, loginData.Password);

            return await Task.FromResult(isAuthenticated);
        }
    }
}