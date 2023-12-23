using Carpool.API.Controllers;
using Carpool.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Carpool.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async Task GetAllUsers_Returns_OkResult_WithUsers()
        {
            var mockUserService = new Mock<IUserService>();
            var expectedUsers = new List<User>
            {
                TestDataGenerator.GenerateRandomUser(),
                TestDataGenerator.GenerateRandomUser(),
                TestDataGenerator.GenerateRandomUser()
            };
            mockUserService.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(expectedUsers);

            var userController = new UserController(mockUserService.Object);
            IActionResult result = await userController.GetAllUsers();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var users = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
            Assert.Equal(expectedUsers.Count, users.Count());
        }

        [Fact]
        public async Task GetAllUsers_Returns_500_OnInternalError()
        {
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(repo => repo.GetAllUsersAsync()).ThrowsAsync(new Exception("Internal server error"));

            var userController = new UserController(mockUserService.Object);
            IActionResult result = await userController.GetAllUsers();

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
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

        [Fact]
        public async Task GetUser_Returns_500_OnInternalError()
        {
            var userId = Guid.NewGuid();
            var mockUserService = new Mock<IUserService>();
            
            mockUserService.Setup(repo => repo.GetUserByIdAsync(userId)).ThrowsAsync(new Exception("Internal server error"));

            var userController = new UserController(mockUserService.Object);

            IActionResult result = await userController.GetUser(userId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}