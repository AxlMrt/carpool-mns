using System.Reflection;
using Bogus;
using Carpool.Application;
using Carpool.Domain.DTOs;
using Carpool.Domain.Entities;
using Carpool.Domain.Roles;
using Castle.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace Carpool.Tests.Utilities
{
    public static class TestDataGenerator
    {
        private static readonly Faker _faker = new Faker();
        public static User GenerateRandomUser()
        {
            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = _faker.Person.FirstName,
                LastName = _faker.Person.LastName,
                Email = _faker.Person.Email,
                Password = _faker.Random.AlphaNumeric(10),
            };
        }

        public static User GenerateRandomAdmin()
        {
            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = _faker.Person.FirstName,
                LastName = _faker.Person.LastName,
                Email = _faker.Person.Email,
                Password = _faker.Random.AlphaNumeric(10),
                Role = Roles.Administrator
            };
        }

        public static RegisterUserDto GenerateValidRegisterUserDto()
        {
            return new RegisterUserDto
            {
                FirstName = _faker.Person.FirstName,
                LastName = _faker.Person.LastName,
                Email = _faker.Person.Email,
                Password = _faker.Random.AlphaNumeric(10),
            };
        }

        public static async Task<string> GenerateToken(string userId, string userRole)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyDirectory = Path.GetDirectoryName(assembly.Location);
            var configPath = Path.Combine(assemblyDirectory, "../../../appsettings.test.json");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(configPath)
                .Build();

            var secretKey = configuration["Jwt:SecretKey"];
            var audience = configuration["Jwt:Audience"];
            var issuer = configuration["Jwt:Issuer"];

            var jwtService = new JwtService(secretKey, audience, issuer);
            return await jwtService.GenerateTokenAsync(userId, userRole);
        }
    }
}