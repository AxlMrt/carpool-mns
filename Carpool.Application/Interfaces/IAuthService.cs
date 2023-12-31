using Carpool.Domain;
using Carpool.Domain.DTOs;
using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterUserAsync(RegisterUserDto user);
        Task<Token> AuthenticateAsync(LoginDto loginData);
        Task LogoutAsync(string token);
    }
}