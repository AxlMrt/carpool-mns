using Carpool.Application.DTO.Car;
using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Application.Utils;
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
            if (cars == null || !cars.Any())
                throw new NotFoundException("No cars found in the database.");

            return cars;
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            return await _carRepository.GetCarByIdAsync(id);
        }

        public async Task<IEnumerable<Car>> GetCarsByUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new BadRequestException("Invalid ID.");

            User user = await _userRepository.GetUserByIdAsync(userId) ?? throw new NotFoundException($"User with ID {userId} not found.");
            return await _carRepository.GetCarsByUserIdAsync(userId);
        }

        public async Task<Car> CreateCarAsync(CreateCarDTO carDto)
        {
            if (carDto == null)
                throw new BadRequestException("Car object cannot be null.");
            
            User user = await _userRepository.GetUserByIdAsync(carDto.OwnerId) ?? throw new NotFoundException($"User with ID {carDto.OwnerId} not found.");
            Car car = ObjectUpdater.MapObject<Car>(carDto);

            if (!ValidationUtils.IsCarValid(car))
                throw new BadRequestException("Invalid car data.");

            await _carRepository.CreateCarAsync(car);
            user.Cars.Add(car);
            await _userRepository.UpdateUserAsync(user);

            return car;
        }

        public async Task<Car> UpdateCarAsync(int id, UpdateCarDTO carDto)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Car car = await _carRepository.GetCarByIdAsync(id) ?? throw new NotFoundException($"Car with ID {id} not found.");

            ObjectUpdater.UpdateObject<Car, UpdateCarDTO>(car, carDto);

            if (!ValidationUtils.IsCarValid(car))
                throw new BadRequestException("Invalid car data.");

            await _carRepository.UpdateCarAsync(car);
            return car;
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Car existingCar = await _carRepository.GetCarByIdAsync(id) ?? throw new NotFoundException($"Car with ID {id} not found.");

            await _carRepository.DeleteCarAsync(id);
            return true;
        }
    }
}
