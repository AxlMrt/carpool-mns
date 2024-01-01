using Carpool.Domain.Entities;

namespace Carpool.Domain.Interfaces
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<Car> GetCarByIdAsync(Guid id);
        Task<IEnumerable<Car>> GetCarsByUserIdAsync(Guid userId);
        Task CreateCarAsync(Car car);
        Task UpdateCarAsync(Car car);
        Task DeleteCarAsync(Guid id);
    }
}