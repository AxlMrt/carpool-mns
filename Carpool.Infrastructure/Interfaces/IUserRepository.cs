using Carpool.Domain.Entities;

namespace Carpool.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid userId);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid userId);
    }
}