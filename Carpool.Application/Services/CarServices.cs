using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Domain.DTOs;
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

        public async Task<Car> CreateCarAsync(CarCreateDto carDto)
        {
            if (carDto is null)
                throw new BadRequestException("Car object cannot be null.");
            
            User user = await _userRepository.GetUserByIdAsync(carDto.OwnerId) ?? throw new NotFoundException($"User with ID {carDto.OwnerId} not found.");

            Car car = new()
            {
                Brand = carDto.Brand,
                Model = carDto.Model,
                Color = carDto.Color,
                LicensePlate = carDto.LicensePlate,
                Year = carDto.Year,
                NumberOfSeats = carDto.NumberOfSeats,
                InsuranceExpirationDate = carDto.InsuranceExpirationDate,
                TechnicalInspectionDate = carDto.TechnicalInspectionDate,
                OwnerId = user.Id
            };
            await _carRepository.CreateCarAsync(car);

            user.Cars.Add(car);

            await _userRepository.UpdateUserAsync(user);

            return car;
        }

        public async Task<Car> UpdateCarAsync(int id, CarUpdateDto carDto)
        {
            if (id < 0)
                throw new BadRequestException("Invalid ID.");

            Car car = await _carRepository.GetCarByIdAsync(carDto.Id) ?? throw new NotFoundException($"Car with ID {id} not found.");
    
            if (!string.IsNullOrEmpty(carDto.Brand) && carDto.Brand != car.Brand)
                car.Brand = carDto.Brand;
            if (!string.IsNullOrEmpty(carDto.Model) && carDto.Model != car.Model)
                car.Model = carDto.Model;
            if (!string.IsNullOrEmpty(carDto.Color) && carDto.Color != car.Color)
                car.Color = carDto.Color;
            if (!string.IsNullOrEmpty(carDto.LicensePlate) && carDto.LicensePlate != car.LicensePlate)
                car.LicensePlate = carDto.LicensePlate;
            if (carDto.Year != car.Year)
                car.Year = carDto.Year;
            if (carDto.NumberOfSeats != car.NumberOfSeats)
                car.NumberOfSeats = carDto.NumberOfSeats;
            if (carDto.InsuranceExpirationDate != car.InsuranceExpirationDate)
                car.InsuranceExpirationDate = carDto.InsuranceExpirationDate;
            if (carDto.TechnicalInspectionDate != car.TechnicalInspectionDate)
                car.TechnicalInspectionDate = carDto.TechnicalInspectionDate;

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