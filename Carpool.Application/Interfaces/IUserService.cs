using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task UpdateUserAsync(int id, User user);
        Task DeleteUserAsync(int id);
    }
}