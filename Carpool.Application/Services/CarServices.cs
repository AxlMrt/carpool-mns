using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;
using Carpool.Infrastructure.Interfaces;

namespace Carpool.Application.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;

        public CarService(ICarRepository carRepository, IUserRepository userRepository)
        {
            _carRepository = carRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            IEnumerable<Car> cars = await _carRepository.GetAllCarsAsync();
            if (cars is null || !cars.Any())
                throw new NotFoundException("No cars found in database.");

            return cars;
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");

            return await _carRepository.GetCarByIdAsync(id);
        }

        public async Task<IEnumerable<Car>> GetCarsByUserIdAsync(int userId)
        {
            if (userId < 0)
                throw new BadRequestException("Empty ID is not allowed.");

            User user = await _userRepository.GetUserByIdAsync(userId) ?? throw new NotFoundException($"User with ID {userId} not found.");
            return await _carRepository.GetCarsByUserIdAsync(userId);
        }

        public async Task<Car> CreateCarAsync(Car car)
        {
            if (car is null)
                throw new BadRequestException("Car object cannot be null.");

            await _carRepository.CreateCarAsync(car);
            return car;
        }

        public async Task<Car> UpdateCarAsync(int id, Car car)
        {
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");

            Car existingCar = await _carRepository.GetCarByIdAsync(car.Id) ?? throw new NotFoundException($"Car with ID {id} not found.");

            car.Id = existingCar.Id;
            await _carRepository.UpdateCarAsync(car);
            return car;
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");

            Car existingCar = await _carRepository.GetCarByIdAsync(id) ?? throw new NotFoundException($"Car with ID {id} not found.");

            await _carRepository.DeleteCarAsync(id);
            return true;
        }
    }
}