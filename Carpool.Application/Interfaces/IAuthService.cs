using Carpool.API.DTOs;

namespace Carpool.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterUserAsync(RegisterUserDto user);
    }
}