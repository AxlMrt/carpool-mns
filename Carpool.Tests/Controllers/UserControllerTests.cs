using Carpool.API.Controllers;
using Carpool.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Carpool.Tests.Controllers
{
    public class UserControllerTests
    {
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
    }
}