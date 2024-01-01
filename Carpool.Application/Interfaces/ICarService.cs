using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface ICarService
    {
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<Car> GetCarByIdAsync(Guid id);
        Task<Car> CreateCarAsync(Car car);
        Task<Car> UpdateCarAsync(Guid id, Car car);
        Task<bool> DeleteCarAsync(Guid id);
    }
}