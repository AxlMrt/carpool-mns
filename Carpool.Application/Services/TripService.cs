using Carpool.Application.DTO.Trip;
using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Application.Utils;
using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;
using Carpool.Infrastructure.Interfaces;

namespace Carpool.Application.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IUserRepository _userRepository;

        public TripService(ITripRepository tripRepository, IUserRepository userRepository)
        {
            _tripRepository = tripRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<TripDTO>> GetAllTripsAsync()
        {
            IEnumerable<Trip> trips = await _tripRepository.GetAllTripsAsync();

            if (trips == null || !trips.Any())
                throw new NotFoundException("No trips found.");

            return trips.Select(u => ObjectUpdater.MapObject<TripDTO>(u));
        }

        public async Task<TripDTO> GetTripByIdAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Trip trip = await _tripRepository.GetTripByIdAsync(id) ?? throw new NotFoundException($"Trip with ID {id} not found.");
            return ObjectUpdater.MapObject<TripDTO>(trip);
        }

        public async Task<TripDTO> CreateTripAsync(CreateTripDTO tripDto)
        {
            if (tripDto is null)
                throw new BadRequestException("Trip object cannot be null.");

            User user = await _userRepository.GetUserByIdAsync(tripDto.DriverId) ?? throw new NotFoundException($"User with ID {tripDto.DriverId} not found.");

            Trip trip = ObjectUpdater.MapObject<Trip>(tripDto);

            await _tripRepository.CreateTripAsync(trip);

            return ObjectUpdater.MapObject<TripDTO>(trip);
        }

        public async Task<TripDTO> UpdateTripAsync(int id, UpdateTripDTO tripDto)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Trip trip = await _tripRepository.GetTripByIdAsync(id) ?? throw new NotFoundException($"Trip with ID {id} not found.");

            ObjectUpdater.UpdateObject<Trip, UpdateTripDTO>(trip, tripDto);

            await _tripRepository.UpdateTripAsync(trip);
            return ObjectUpdater.MapObject<TripDTO>(trip);
        }

        public async Task<bool> DeleteTripAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Trip existingTrip = await _tripRepository.GetTripByIdAsync(id) ?? throw new NotFoundException($"Trip with ID {id} not found.");

            await _tripRepository.DeleteTripAsync(id);
            return true;
        }
    }
}
