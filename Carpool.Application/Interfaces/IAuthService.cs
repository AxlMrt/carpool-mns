using Carpool.Domain.DTOs;

namespace Carpool.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterUserAsync(RegisterUserDto user);
    }
}