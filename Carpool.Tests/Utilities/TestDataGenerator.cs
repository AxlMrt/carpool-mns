using Bogus;
using Carpool.Domain.DTOs;
using Carpool.Domain.Entities;

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
    }
}