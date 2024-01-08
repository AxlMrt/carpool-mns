using Microsoft.AspNetCore.Mvc;
using Carpool.Application.Interfaces;
using Carpool.Application.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Carpool.Domain.DTO.User;
using Carpool.Domain.Common;

namespace Carpool.API.Controllers
{
    public class UserController : BaseApiController, IExceptionFilter
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
            IEnumerable<UserDTO> users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            UserDTO user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDTO user)
        {
            await _userService.UpdateUserAsync(id, user);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        [NonAction]
        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
            int statusCode = 500;

            if (exception is NotFoundException)
            {
                statusCode = 404;
            }
            else if (exception is BadRequestException || exception is ArgumentException)
            {
                statusCode = 400;
            }

            context.Result = new ObjectResult(exception.Message)
            {
                StatusCode = statusCode
            };
        }
    }
}
