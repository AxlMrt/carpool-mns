using Carpool.Domain.DTOs;
using Carpool.Domain.Entities;
using Carpool.Infrastructure.Interfaces;
using Carpool.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

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
            User newUser = new()
            {
                LastName = user.LastName,
                FirstName = user.FirstName,
                Email = user.Email,
                Password = user.Password,
            };
    
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> FindUserAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}