using Carpool.Domain;
using Carpool.Domain.DTOs;

namespace Carpool.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterUserAsync(RegisterUserDto user);
        Task<bool> AuthenticateAsync(LoginDto loginData);
    }
}