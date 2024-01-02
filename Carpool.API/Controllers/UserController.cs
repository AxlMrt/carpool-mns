using System.Security.Authentication;
using System.Security.Claims;
using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;
using Carpool.Domain.Roles;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = Roles.Administrator)]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                if (!User.IsInRole(Roles.Administrator)) // For testing purpose
                    return Forbid();

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

        [Authorize(Roles = Roles.Administrator)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                if (!User.IsInRole(Roles.Administrator)) // For testing purpose
                    return Forbid();

                if (id == Guid.Empty)
                    return BadRequest("Invalid user ID.");

                User user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching the user.");
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, User user)
        {
            try
            {
                if (id != user.Id || user.Role != Roles.Administrator)
                    return Forbid();
                
                await _userService.UpdateUserAsync(user);
                return NoContent();
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(InvalidCredentialException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }

        [Authorize]
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
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NotAllowedException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while removing the user.");
            }
        }
    }
}