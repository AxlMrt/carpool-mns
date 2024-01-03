using Carpool.Domain.Entities;
using Carpool.Infrastructure.Interfaces;
using Carpool.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Carpool.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CarpoolDbContext _dbContext;

        public UserRepository(CarpoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.Include(u => u.Addresses).Include(u => u.Cars).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateUserAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(u=> u.Id == id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}