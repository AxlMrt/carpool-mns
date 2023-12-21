using Carpool.API.DTOs;

namespace Carpool.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task RegisterUserAsync(RegisterUserDto user);
    }
}