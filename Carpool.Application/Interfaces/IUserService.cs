using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid id);
        Task UpdateUserAsync(Guid id, User user);
        Task DeleteUserAsync(Guid id);
    }
}