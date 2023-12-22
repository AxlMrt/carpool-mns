using Carpool.Domain.DTOs;
using Carpool.Domain.Entities;
using Carpool.Infrastructure.Interfaces;
using Carpool.Infrastructure.Context;

namespace Carpool.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly CarpoolDbContext _dbContext;

        public AuthRepository(CarpoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task RegisterUserAsync(RegisterUserDto user)
        {
            User newUser = new User
            {
                LastName = user.LastName,
                FirstName = user.FirstName,
                Email = user.LastName,
                Password = user.Password,
            };
    
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
        }
    }
}