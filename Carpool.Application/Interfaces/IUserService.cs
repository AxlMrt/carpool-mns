using Carpool.Domain.DTO.User;
using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int id);
        Task UpdateUserAsync(int id, UpdateUserDTO user);
        Task DeleteUserAsync(int id);
    }
}