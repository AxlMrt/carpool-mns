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

        public async Task<User> GetUserByIdAsync(int id)
        {
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");

            return await _userRepository.GetUserByIdAsync(id)  ?? throw new NotFoundException($"User with ID {id} not found.");
        }

        public async Task UpdateUserAsync(int id, User updatedUserData)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid user ID.");

            User existingUser = await _userRepository.GetUserByIdAsync(id) ?? throw new NotFoundException($"User with ID {id} not found.");

            if (existingUser.Role != Roles.Administrator)
                throw new NotAllowedException("You are not allowed to update this user.");

            existingUser.FirstName = updatedUserData.FirstName ?? existingUser.FirstName;
            existingUser.LastName = updatedUserData.LastName ?? existingUser.LastName;
            existingUser.Email = updatedUserData.Email ?? existingUser.Email;

            if (!string.IsNullOrEmpty(updatedUserData.Password))
            {
                bool validPassword = _passwordHasherService.VerifyPassword(existingUser.Password, updatedUserData.Password);
                if (!validPassword)
                    throw new InvalidCredentialException("Wrong credentials.");

                existingUser.Password = _passwordHasherService.HashPassword(updatedUserData.Password);
            }

            await _userRepository.UpdateUserAsync(existingUser);
        }

        public async Task DeleteUserAsync(int id)
        {
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");

            User existingUser = await GetUserByIdAsync(id) ?? throw new NotFoundException($"User with ID {id} not found.");

            if (existingUser.Role != Roles.Administrator || existingUser.Id != id)
                throw new NotAllowedException("You are not allowed to delete this user.");

            await _userRepository.DeleteUserAsync(id);
        }
    }
}