using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid userId);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid userId);
    }
}