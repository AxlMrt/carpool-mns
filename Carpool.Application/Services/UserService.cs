using System.Security.Authentication;
using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Application.Utils;
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
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            return await _userRepository.GetUserByIdAsync(id)  ?? throw new NotFoundException($"User with ID {id} not found.");
        }

        public async Task UpdateUserAsync(int id, User updatedUserData)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid user ID.");

            User existingUser = await _userRepository.GetUserByIdAsync(id) ?? throw new NotFoundException($"User with ID {id} not found.");

            if (existingUser.Role != Roles.Administrator)
                throw new NotAllowedException("You are not allowed to update this user.");

            if (updatedUserData.Email != null && !ValidationUtils.IsValidEmail(updatedUserData.Email))
                throw new BadRequestException("Invalid email format.");

            if (updatedUserData.Password != null && !ValidationUtils.IsStrongPassword(updatedUserData.Password))
                throw new BadRequestException("Password should be at least 8 characters long, contain at least one uppercase, one lowercase, and one special character.");


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
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            User existingUser = await GetUserByIdAsync(id) ?? throw new NotFoundException($"User with ID {id} not found.");

            if (existingUser.Role != Roles.Administrator || existingUser.Id != id)
                throw new NotAllowedException("You are not allowed to delete this user.");

            await _userRepository.DeleteUserAsync(id);
        }
    }
}