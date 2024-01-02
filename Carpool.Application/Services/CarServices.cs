using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;

namespace Carpool.Application.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            return await _carRepository.GetAllCarsAsync();
        }

        public async Task<Car> GetCarByIdAsync(Guid id)
        {
            return await _carRepository.GetCarByIdAsync(id);
        }

        public async Task<IEnumerable<Car>> GetCarsByUserIdAsync(Guid userId)
        {
            return await _carRepository.GetCarsByUserIdAsync(userId);
        }

        public async Task<Car> CreateCarAsync(Car car)
        {
            await _carRepository.CreateCarAsync(car);
            return car;
        }

        public async Task<Car> UpdateCarAsync(Guid id, Car car)
        {
            var existingCar = await _carRepository.GetCarByIdAsync(id) ?? throw new NotFoundException($"Car with ID {id} not found.");

            car.Id = existingCar.Id;
            await _carRepository.UpdateCarAsync(car);
            return car;
        }

        public async Task<bool> DeleteCarAsync(Guid id)
        {
            var existingCar = await _carRepository.GetCarByIdAsync(id) ?? throw new NotFoundException($"Car with ID {id} not found.");

            await _carRepository.DeleteCarAsync(id);
            return true;
        }
    }
}