using System.Security.Authentication;
using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;
using Carpool.Domain.Roles;
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
            IEnumerable<User> users = await _userRepository.GetAllUsersAsync();
            
            if (users is null || !users.Any())
                throw new NotFoundException("No users found in database.");
            
            return users;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Empty ID is not allowed.");

            return await _userRepository.GetUserByIdAsync(id)  ?? throw new NotFoundException($"User with ID {id} not found.");
        }

        public async Task UpdateUserAsync(Guid id, User user)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Empty ID is not allowed.");

            User existingUser = await GetUserByIdAsync(user.Id) ?? throw new NotFoundException($"User with ID {user.Id} not found.");

            if (existingUser.Id != user.Id || user.Role != Roles.Administrator)
                throw new NotAllowedException("You are not allowed to update this user.");

            bool valid_password = _passwordHasherService.VerifyPassword(existingUser.Password, user.Password);
            
            if (!valid_password)
                throw new InvalidCredentialException($"Wrong credentials.");

            user.Password = _passwordHasherService.HashPassword(user.Password);
    
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Empty ID is not allowed.");

            User existingUser = await GetUserByIdAsync(id) ?? throw new NotFoundException($"User with ID {id} not found.");

            if (existingUser.Role != Roles.Administrator || existingUser.Id != id)
                throw new NotAllowedException("You are not allowed to delete this user.");

            await _userRepository.DeleteUserAsync(id);
        }
    }
}