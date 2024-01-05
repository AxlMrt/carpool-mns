using Carpool.Application.DTO.Trip;
using Carpool.Domain.DTOs;
using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface ITripService
    {
        Task<IEnumerable<Trip>> GetAllTripsAsync();
        Task<Trip> GetTripByIdAsync(int id);
        Task<Trip> CreateTripAsync(CreateTripDTO trip);
        Task<Trip> UpdateTripAsync(int id, UpdateTripDTO trip);
        Task<bool> DeleteTripAsync(int id);
    }
}