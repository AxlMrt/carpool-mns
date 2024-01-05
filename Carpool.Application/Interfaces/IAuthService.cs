using Carpool.Application.DTO.Auth;
using Carpool.Domain;
using Carpool.Domain.DTOs;
using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterUserAsync(RegisterUserDTO user);
        Task<Token> AuthenticateAsync(LoginDTO loginData);
        Task LogoutAsync(string token);
        Task<string> RefreshTokenAsync(string token);
    }
}