using Carpool.Application.Exceptions;
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
            IEnumerable<Trip> trips = await _tripRepository.GetAllTripsAsync();

            if (trips == null || !trips.Any())
                throw new NotFoundException("No trips found.");

            return trips;
        }

        public async Task<Trip> GetTripByIdAsync(int id)
        {
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");

            Trip trip = await _tripRepository.GetTripByIdAsync(id) ?? throw new NotFoundException($"Trip with ID {id} not found.");
            return trip;
        }

        public async Task<Trip> CreateTripAsync(Trip trip)
        {
            if (trip == null)
                throw new BadRequestException("Trip object cannot be null.");

            await _tripRepository.CreateTripAsync(trip);
            return trip;
        }

        public async Task<Trip> UpdateTripAsync(int id, Trip trip)
        {
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");

            Trip existingTrip = await GetTripByIdAsync(id) ?? throw new NotFoundException($"Trip with ID {id} not found.");

            trip.Id = existingTrip.Id;
            await _tripRepository.UpdateTripAsync(trip);
            return trip;
        }

        public async Task<bool> DeleteTripAsync(int id)
        {
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");

            Trip existingTrip = await GetTripByIdAsync(id) ?? throw new NotFoundException($"Trip with ID {id} not found.");

            await _tripRepository.DeleteTripAsync(id);
            return true;
        }
    }
}
