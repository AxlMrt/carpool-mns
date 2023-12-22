using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;
using Carpool.Infrastructure.Interfaces;

namespace Carpool.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasherService;

        public UserService(IUserRepository userRepository, IPasswordHasherService passwordHasherService)
        {
            _userRepository = userRepository;
            _passwordHasherService = passwordHasherService;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _userRepository.GetUserByIdAsync(userId)  ?? throw new UserNotFoundException($"User with ID {userId} not found.");
        }

        public async Task UpdateUserAsync(User user)
        {
            User existingUser = await GetUserByIdAsync(user.Id) ?? throw new UserNotFoundException($"User with ID {user.Id} not found.");

            if (existingUser.Password != user.Password)
                user.Password = _passwordHasherService.HashPassword(user.Password);
    
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            User existingUser = await GetUserByIdAsync(userId) ?? throw new UserNotFoundException($"User with ID {userId} not found.");
            await _userRepository.DeleteUserAsync(userId);
        }
    }
}