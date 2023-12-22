using Carpool.API.DTOs;

namespace Carpool.Infrastructure.Interfaces
{
    public interface IAuthRepository
    {
        Task RegisterUserAsync(RegisterUserDto user);
    }
}