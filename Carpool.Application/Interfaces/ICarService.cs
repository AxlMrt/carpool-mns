using Carpool.Application.DTO.Car;
using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface ICarService
    {
        Task<IEnumerable<CarDTO>> GetAllCarsAsync();
        Task<CarDTO> GetCarByIdAsync(int id);
        Task<IEnumerable<CarDTO>> GetCarsByUserIdAsync(int userId);
        Task<CarDTO> CreateCarAsync(CreateCarDTO car);
        Task<CarDTO> UpdateCarAsync(int id, UpdateCarDTO car);
        Task<bool> DeleteCarAsync(int id);
    }
}