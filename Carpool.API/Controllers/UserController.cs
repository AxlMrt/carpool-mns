using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Carpool.API.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasherService _passwordHasherService;

        public UserController(IUserService userService, IPasswordHasherService passwordHasherService)
        {
            _userService = userService;
            _passwordHasherService = passwordHasherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            IEnumerable<User> users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            User user = await _userService.GetUserByIdAsync(userId);
            if (user is null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            user.Password = _passwordHasherService.HashPassword(user.Password);

            await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid userId, User user)
        {
            if (userId != user.Id)
                return BadRequest();
            
            User existingUser = await _userService.GetUserByIdAsync(userId);

            if (existingUser is null) return NotFound("User not found");

            if (existingUser.Password != user.Password)
                user.Password = _passwordHasherService.HashPassword(user.Password);
            
            await _userService.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            await _userService.DeleteUserAsync(userId);
            return NoContent();
        }
    }
}