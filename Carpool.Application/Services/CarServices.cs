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

        public async Task<IEnumerable<CarDTO>> GetAllCarsAsync()
        {
            IEnumerable<Car> cars = await _carRepository.GetAllCarsAsync();
            if (cars == null || !cars.Any())
                throw new NotFoundException("No cars found in the database.");

            return cars.Select(u => ObjectUpdater.MapObject<CarDTO>(u));
        }

        public async Task<CarDTO> GetCarByIdAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Car car = await _carRepository.GetCarByIdAsync(id);
            return ObjectUpdater.MapObject<CarDTO>(car);
        }

        public async Task<IEnumerable<CarDTO>> GetCarsByUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new BadRequestException("Invalid ID.");

            User user = await _userRepository.GetUserByIdAsync(userId) ?? throw new NotFoundException($"User with ID {userId} not found.");
            IEnumerable<Car> cars = await _carRepository.GetCarsByUserIdAsync(userId);

            return cars.Select(u => ObjectUpdater.MapObject<CarDTO>(u));
        }

        public async Task<CarDTO> CreateCarAsync(CreateCarDTO carDto)
        {
            if (carDto == null)
                throw new BadRequestException("Car object cannot be null.");
            
            User user = await _userRepository.GetUserByIdAsync(carDto.OwnerId) ?? throw new NotFoundException($"User with ID {carDto.OwnerId} not found.");
            Car car = ObjectUpdater.MapObject<Car>(carDto);

            string validationResult = ValidationUtils.IsCarValid(car);
            if (validationResult != "Valid")
                throw new BadRequestException(validationResult);

            await _carRepository.CreateCarAsync(car);
            user.Cars.Add(car);
            await _userRepository.UpdateUserAsync(user);

            return ObjectUpdater.MapObject<CarDTO>(car);
        }

        public async Task<CarDTO> UpdateCarAsync(int id, UpdateCarDTO carDto)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Car car = await _carRepository.GetCarByIdAsync(id) ?? throw new NotFoundException($"Car with ID {id} not found.");

            ObjectUpdater.UpdateObject<Car, UpdateCarDTO>(car, carDto);

            string validationResult = ValidationUtils.IsCarValid(car);
            if (validationResult != "Valid")
                throw new BadRequestException(validationResult);

            await _carRepository.UpdateCarAsync(car);
            return ObjectUpdater.MapObject<CarDTO>(car);
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Car existingCar = await _carRepository.GetCarByIdAsync(id) ?? throw new NotFoundException($"Car with ID {id} not found.");
            User user = await _userRepository.GetUserByIdAsync(existingCar.OwnerId) ?? throw new NotFoundException($"User with car ID {id} not found.");
            await _carRepository.DeleteCarAsync(id);
            user.Cars.Remove(existingCar);
            return true;
        }
    }
}
