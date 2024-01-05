namespace Carpool.Tests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetUserById_ValidId_ReturnsUser()
        {
            var existingUserId = Guid.NewGuid();
            var expectedUser = TestDataGenerator.GenerateRandomUser();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(existingUserId))
                .ReturnsAsync(expectedUser);

            var userService = new UserService(userRepositoryMock.Object, It.IsAny<IPasswordHasherService>());

            var user = await userService.GetUserByIdAsync(existingUserId);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(expectedUser.Id, user.Id);
            Assert.Equal(expectedUser.FirstName, user.FirstName); 
            Assert.Equal(expectedUser.LastName, user.LastName);
            Assert.Equal(expectedUser.Email, user.Email);
        }
    }
}