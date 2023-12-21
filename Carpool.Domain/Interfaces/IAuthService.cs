using Carpool.API.DTOs;

namespace Carpool.Domain.Interfaces
{
    public interface IAuthService
    {
        Task RegisterUserAsync(RegisterUserDto user);
    }
}