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
        public async Task<IActionResult> GetUser(Guid id)
        {
            User user = await _userService.GetUserByIdAsync(id);
            if (user is null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, User user)
        {
            if (id != user.Id)
                return BadRequest();
            
            User existingUser = await _userService.GetUserByIdAsync(id);

            if (existingUser is null) return NotFound("User not found");

            if (existingUser.Password != user.Password)
                user.Password = _passwordHasherService.HashPassword(user.Password);
            
            await _userService.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}