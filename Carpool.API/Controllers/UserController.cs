using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Carpool.API.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
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
            await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid userId, User user)
        {
            if (userId != user.Id)
                return BadRequest();
            
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