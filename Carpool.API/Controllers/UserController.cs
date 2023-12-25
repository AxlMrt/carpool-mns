using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;
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
            try
            {
                IEnumerable<User> users = await _userService.GetAllUsersAsync();

                if (users is null || !users.Any())
                    return NotFound("No users found.");

                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching the users list.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Invalid user ID.");

                User user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch(UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching the user.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, User user)
        {
            try
            {
                if (id != user.Id)
                    return BadRequest("IDs does not match.");
                
                await _userService.UpdateUserAsync(user);
                return NoContent();
            }
            catch(UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Invalid user ID.");

                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while removing the user.");
            }
        }
    }
}