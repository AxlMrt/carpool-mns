using Carpool.API.DTOs;
using Carpool.Domain.Interfaces;

namespace Carpool.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task RegisterUserAsync(RegisterUserDto user)
        {
            await _authRepository.RegisterUserAsync(user);
        }
    }
}