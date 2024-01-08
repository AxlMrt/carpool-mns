using System.Net;
using System.Net.Http.Headers;
using Carpool.Domain.Entities;

public class UserControllerTests
{
    private readonly HttpClient _client;

    public UserControllerTests()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5011/") };
    }

    [Fact]
    public async Task GetAllUsers_ReturnsSuccessStatusCode_ForAdmin()
    {
        User admin = TestDataGenerator.GenerateRandomAdmin();
        string token = await TestDataGenerator.GenerateToken(admin.Id.ToString(), admin.Role);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Arrange
        HttpRequestMessage request = new(HttpMethod.Get, "api/user");

        // Act
        HttpResponseMessage response = await _client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsForbidden_ForNonAdminUser()
    {
        User nonAdminUser = TestDataGenerator.GenerateRandomUser();
        string token = await TestDataGenerator.GenerateToken(nonAdminUser.Id.ToString(), nonAdminUser.Role);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpRequestMessage request = new(HttpMethod.Get, "api/user");
        HttpResponseMessage response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsUnauthorized_ForUnauthenticatedUser()
    {
        HttpRequestMessage request = new(HttpMethod.Get, "api/user");
        HttpResponseMessage response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
}
