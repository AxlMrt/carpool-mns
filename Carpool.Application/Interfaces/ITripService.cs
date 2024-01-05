using Carpool.Application.DTO.Trip;
using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface ITripService
    {
        Task<IEnumerable<TripDTO>> GetAllTripsAsync();
        Task<TripDTO> GetTripByIdAsync(int id);
        Task<TripDTO> CreateTripAsync(CreateTripDTO trip);
        Task<TripDTO> UpdateTripAsync(int id, UpdateTripDTO trip);
        Task<bool> DeleteTripAsync(int id);
    }
}