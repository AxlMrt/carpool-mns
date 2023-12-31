using Carpool.API.Controllers;
using Carpool.Application.Exceptions;
using Carpool.Domain.Entities;
using Carpool.Domain.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Carpool.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async Task GetAllUsers_Returns_OkResult_WithUsers()
        {
            User adminUser = TestDataGenerator.GenerateRandomAdmin();
        
            var mockUserService = new Mock<IUserService>();
            var mockHttpContext = new Mock<HttpContext>();

            var expectedUsers = new List<User>
            {
                TestDataGenerator.GenerateRandomUser(),
                TestDataGenerator.GenerateRandomUser(),
                TestDataGenerator.GenerateRandomUser()
            };
            mockUserService.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(expectedUsers);
            mockHttpContext.Setup(c => c.User.IsInRole(Roles.Administrator)).Returns(true);

            var userController = new UserController(mockUserService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            IActionResult result = await userController.GetAllUsers();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var users = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
            Assert.Equal(expectedUsers.Count, users.Count());
        }

        [Fact]
        public async Task GetAllUsers_Returns_Forbidden_WhenUserIsNotAdmin() // Je dois tester tous les cas ? User, Moderateur.. etc ?
        {
            User regularUser = TestDataGenerator.GenerateRandomUser();

            var mockUserService = new Mock<IUserService>();
            var mockHttpContext = new Mock<HttpContext>();

            mockHttpContext.Setup(c => c.User.IsInRole(Roles.Administrator)).Returns(false);

            var userController = new UserController(mockUserService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            IActionResult result = await userController.GetAllUsers();

            Assert.IsType<ForbidResult>(result);
        }



        [Fact(Skip = "Update in progress to handle tokens")]
        public async Task GetAllUsers_Returns_404_WhenNoUsers()
        {
            var mockUserService = new Mock<IUserService>();
            var emptyUsersList = new List<User>();
            mockUserService.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(emptyUsersList);

            var userController = new UserController(mockUserService.Object);

            IActionResult result = await userController.GetAllUsers();

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact(Skip = "Update in progress to handle tokens")]
        public async Task GetAllUsers_Returns_500_OnInternalError()
        {
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(repo => repo.GetAllUsersAsync()).ThrowsAsync(new Exception("Internal server error"));

            var userController = new UserController(mockUserService.Object);
            IActionResult result = await userController.GetAllUsers();

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal("An error occurred while fetching the users list.", objectResult.Value);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact(Skip = "Update in progress to handle tokens")]
        public async Task GetUser_Returns_OkResult_WhenValidIdProvided()
        {
            Guid userId = Guid.NewGuid(); 
            var mockUserService = new Mock<IUserService>();
            var expectedUser = TestDataGenerator.GenerateRandomUser();
        
            mockUserService.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync(expectedUser);

            var userController = new UserController(mockUserService.Object);

            IActionResult result = await userController.GetUser(userId);

            Assert.Equal(typeof(OkObjectResult), result.GetType());
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact(Skip = "Update in progress to handle tokens")]
        public async Task GetUser_Returns_BadRequest_WhenBadRequest()
        {
            Guid userId = Guid.NewGuid(); 
            var invalidUserId = Guid.Empty;

            var mockUserService = new Mock<IUserService>();
            var userController = new UserController(mockUserService.Object);

            IActionResult result = await userController.GetUser(invalidUserId);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid user ID.", objectResult.Value);
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact(Skip = "Update in progress to handle tokens")]
        public async Task GetUser_Returns_404_WhenUserNotFound()
        {
            Guid nonExistentUserId = Guid.NewGuid();
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(repo => repo.GetUserByIdAsync(nonExistentUserId)).ThrowsAsync(new UserNotFoundException("User not found"));

            var userController = new UserController(mockUserService.Object);

            IActionResult result = await userController.GetUser(nonExistentUserId);

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found", objectResult.Value);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact(Skip = "Update in progress to handle tokens")]
        public async Task GetUser_Returns_500_OnInternalError()
        {
            Guid userId = Guid.NewGuid();
            var mockUserService = new Mock<IUserService>();
            
            mockUserService.Setup(repo => repo.GetUserByIdAsync(userId)).ThrowsAsync(new Exception("Internal server error"));

            var userController = new UserController(mockUserService.Object);

            IActionResult result = await userController.GetUser(userId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal("An error occurred while fetching the user.", objectResult.Value);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact(Skip = "Update in progress to handle tokens")]
        public async Task UpdateUser_Returns_NoContent_OnSuccess()
        {
            User validUser = TestDataGenerator.GenerateRandomUser();
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(repo => repo.UpdateUserAsync(validUser));

            var userController = new UserController(mockUserService.Object);

            IActionResult result = await userController.UpdateUser(validUser.Id, validUser);

            var objectResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, objectResult.StatusCode);
        }

        [Fact(Skip = "Update in progress to handle tokens")]
        public async Task UpdateUser_Returns_400_WhenBadRequest()
        {
            Guid userId = Guid.NewGuid();
            User invalidUser = new User();
            var mockUserService = new Mock<IUserService>();

            var userController = new UserController(mockUserService.Object);
            IActionResult result = await userController.UpdateUser(userId, invalidUser);
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact(Skip = "Update in progress to handle tokens")]
        public async Task UpdateUser_Returns_404_WhenUserNotFound()
        {
            User nonExistingUser = TestDataGenerator.GenerateRandomUser();
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(repo => repo.UpdateUserAsync(nonExistingUser)).ThrowsAsync(new UserNotFoundException("User not found"));

            var userController = new UserController(mockUserService.Object);

            IActionResult result = await userController.UpdateUser(nonExistingUser.Id, nonExistingUser);

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found", objectResult.Value);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact(Skip = "Update in progress to handle tokens")]
        public async Task DeleteUser_Returns_NoContent_OnSuccess()
        {
            Guid userId = Guid.NewGuid();
            var mockUserService = new Mock<IUserService>();
            
            mockUserService.Setup(repo => repo.DeleteUserAsync(userId));

            var userController = new UserController(mockUserService.Object);
            IActionResult result = await userController.DeleteUser(userId);

            var objectResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, objectResult.StatusCode);
        }

        [Fact(Skip = "Update in progress to handle tokens")]
        public async Task DeleteUser_Returns_BadRequest_WhenBadRequest()
        {
            Guid userId = Guid.NewGuid();
            var invalidUserId = Guid.Empty;

            var mockUserService = new Mock<IUserService>();
            var userController = new UserController(mockUserService.Object);

            IActionResult result = await userController.DeleteUser(invalidUserId);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid user ID.", objectResult.Value);
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact(Skip = "Update in progress to handle tokens")]
        public async Task DeleteUser_Returns_404_WhenUserNotFound()
        {
            Guid nonExistentUserId = Guid.NewGuid();
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(repo => repo.DeleteUserAsync(nonExistentUserId)).ThrowsAsync(new UserNotFoundException("User not found"));

            var userController = new UserController(mockUserService.Object);

            IActionResult result = await userController.DeleteUser(nonExistentUserId);

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found", objectResult.Value);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact(Skip = "Update in progress to handle tokens")]
        public async Task DeleteUser_Returns_500_OnInternalError()
        {
            Guid userId = Guid.NewGuid();
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(repo => repo.DeleteUserAsync(userId)).ThrowsAsync(new Exception("Internal server error"));

            var userController = new UserController(mockUserService.Object);
            IActionResult result = await userController.DeleteUser(userId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal("An error occurred while removing the user.", objectResult.Value);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}