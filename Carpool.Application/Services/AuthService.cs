using Carpool.Domain.DTOs;
using Carpool.Application.Interfaces;
using Carpool.Infrastructure.Interfaces;
using Carpool.Domain;
using Carpool.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Carpool.Application.Exceptions;
using System.Security.Authentication;

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
            if (user is null)
                throw new BadRequestException("User object cannot be null");

            user.Password = _passwordHasherService.HashPassword(user.Password);
            await _authRepository.RegisterUserAsync(user);
        }

        public async Task<Token> AuthenticateAsync(LoginDto loginData)
        {
            User user = await _authRepository.FindUserAsync(loginData.Email) ?? throw new InvalidCredentialException("Invalid email.");

            if (!_passwordHasherService.VerifyPassword(user.Password, loginData.Password))
                throw new InvalidCredentialException("Invalid password.");

            string token = await _jwtService.GenerateTokenAsync(user.Id.ToString(), user.Role);

            return await _tokenManagerService.AddTokenAsync(user, token);
        }

        public async Task LogoutAsync(string token)
        {
            await _tokenManagerService.RemoveTokenAsync(token);
        }

        public async Task<string> RefreshTokenAsync(string token)
        {
            string newToken = await _jwtService.RefreshTokenAsync(token);
            await _tokenManagerService.UpdateTokenAsync(token, newToken);

            return newToken;
        }
    }
}