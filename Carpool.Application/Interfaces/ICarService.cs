using Carpool.Application.DTO.Car;
using Carpool.Domain.DTOs;
using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface ICarService
    {
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<Car> GetCarByIdAsync(int id);
        Task<IEnumerable<Car>> GetCarsByUserIdAsync(int userId);
        Task<Car> CreateCarAsync(CreateCarDTO car);
        Task<Car> UpdateCarAsync(int id, UpdateCarDTO car);
        Task<bool> DeleteCarAsync(int id);
    }
}