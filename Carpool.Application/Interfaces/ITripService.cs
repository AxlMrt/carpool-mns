using Carpool.Domain.DTOs;
using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface ITripService
    {
        Task<IEnumerable<Trip>> GetAllTripsAsync();
        Task<Trip> GetTripByIdAsync(int id);
        Task<Trip> CreateTripAsync(TripCreateDto trip);
        Task<Trip> UpdateTripAsync(int id, TripUpdateDto trip);
        Task<bool> DeleteTripAsync(int id);
    }
}