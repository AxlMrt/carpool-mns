using Carpool.Domain.DTOs;
using Carpool.Domain.Entities;

namespace Carpool.Infrastructure.Interfaces
{
    public interface IAuthRepository
    {
        Task RegisterUserAsync(RegisterUserDto user);
        Task<User> FindUserAsync(string email);
    }
}