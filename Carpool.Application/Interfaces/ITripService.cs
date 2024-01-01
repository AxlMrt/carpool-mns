using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface ITripService
    {
        Task<IEnumerable<Trip>> GetAllTripsAsync();
        Task<Trip> GetTripByIdAsync(Guid id);
        Task<Trip> CreateTripAsync(Trip trip);
        Task<Trip> UpdateTripAsync(Guid id, Trip trip);
        Task<bool> DeleteTripAsync(Guid id);
    }
}