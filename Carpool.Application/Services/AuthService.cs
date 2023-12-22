using Carpool.Domain.DTOs;
using Carpool.Application.Interfaces;
using Carpool.Infrastructure.Interfaces;

namespace Carpool.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        public AuthService(IAuthRepository authRepository, IPasswordHasherService passwordHasherService)
        {
            _authRepository = authRepository;
            _passwordHasherService = passwordHasherService;
        }

        public async Task RegisterUserAsync(RegisterUserDto user)
        {
            user.Password = _passwordHasherService.HashPassword(user.Password);
            await _authRepository.RegisterUserAsync(user);
        }
    }
}