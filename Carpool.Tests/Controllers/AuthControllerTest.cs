using Carpool.API.Controllers;
using Carpool.Application;
using Carpool.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Carpool.Tests;

public class AuthControllerTest
{
    [Fact]
    public async Task RegisterUser_Returns_200_Ok_WhenRegistrationSuccessful()
    {
        var mockAuthService = new Mock<IAuthService>();
        var mockJwtService = new Mock<IJwtService>();
        mockAuthService.Setup(auth => auth.RegisterUserAsync(It.IsAny<RegisterUserDto>())).Returns(Task.FromResult(true));


        var userController = new AuthController(mockAuthService.Object, mockJwtService.Object);
        var validUser = TestDataGenerator.GenerateValidRegisterUserDto();

        IActionResult result = await userController.RegisterUser(validUser);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("User registered successfully.", okResult.Value);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task RegisterUser_Returns_400_BadRequest_OnInvalidData()
    {
        var mockAuthService = new Mock<IAuthService>();
        var mockJwtService = new Mock<IJwtService>();
        var userController = new AuthController(mockAuthService.Object, mockJwtService.Object);
        userController.ModelState.AddModelError("Property", "Error message"); 

        IActionResult result = await userController.RegisterUser(null);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid registration data.", badRequestResult.Value);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task RegisterUser_Returns_500_InternalServerError_OnException()
    {
        var mockAuthService = new Mock<IAuthService>();
        var mockJwtService = new Mock<IJwtService>();
        mockAuthService.Setup(auth => auth.RegisterUserAsync(It.IsAny<RegisterUserDto>())).ThrowsAsync(new Exception());

        var userController = new AuthController(mockAuthService.Object, mockJwtService.Object);
        var validUser = TestDataGenerator.GenerateValidRegisterUserDto();

        IActionResult result = await userController.RegisterUser(validUser);

        var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal("An error occurred while registering the user.", internalServerErrorResult.Value);
        Assert.Equal(500, internalServerErrorResult.StatusCode);
    }
}