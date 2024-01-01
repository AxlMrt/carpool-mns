using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;

namespace Carpool.Application.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;

        public TripService(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public async Task<IEnumerable<Trip>> GetAllTripsAsync()
        {
            return await _tripRepository.GetAllTripsAsync();
        }

        public async Task<Trip> GetTripByIdAsync(Guid id)
        {
            return await _tripRepository.GetTripByIdAsync(id);
        }

        public async Task<Trip> CreateTripAsync(Trip trip)
        {
            await _tripRepository.CreateTripAsync(trip);
            return trip;
        }

        public async Task<Trip> UpdateTripAsync(Guid id, Trip trip)
        {
            var existingTrip = await _tripRepository.GetTripByIdAsync(id);
            if (existingTrip == null)
                return null;

            trip.Id = existingTrip.Id;
            await _tripRepository.UpdateTripAsync(trip);
            return trip;
        }

        public async Task<bool> DeleteTripAsync(Guid id)
        {
            var existingTrip = await _tripRepository.GetTripByIdAsync(id);
            if (existingTrip == null)
                return false;

            await _tripRepository.DeleteTripAsync(id);
            return true;
        }
    }
}