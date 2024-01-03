using Carpool.Domain.DTOs;
using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface ICarService
    {
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<Car> GetCarByIdAsync(int id);
        Task<IEnumerable<Car>> GetCarsByUserIdAsync(int userId);
        Task<Car> CreateCarAsync(CarCreateDto car);
        Task<Car> UpdateCarAsync(int id, CarUpdateDto car);
        Task<bool> DeleteCarAsync(int id);
    }
}