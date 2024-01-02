using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface ITripService
    {
        Task<IEnumerable<Trip>> GetAllTripsAsync();
        Task<Trip> GetTripByIdAsync(int id);
        Task<Trip> CreateTripAsync(Trip trip);
        Task<Trip> UpdateTripAsync(int id, Trip trip);
        Task<bool> DeleteTripAsync(int id);
    }
}