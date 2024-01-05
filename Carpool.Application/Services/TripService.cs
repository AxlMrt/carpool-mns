using Carpool.Application.DTO.Trip;
using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Application.Utils;
using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;
using Carpool.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<Trip>> GetAllTripsAsync()
        {
            IEnumerable<Trip> trips = await _tripRepository.GetAllTripsAsync();

            if (trips == null || !trips.Any())
                throw new NotFoundException("No trips found.");

            return trips;
        }

        public async Task<Trip> GetTripByIdAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Trip trip = await _tripRepository.GetTripByIdAsync(id) ?? throw new NotFoundException($"Trip with ID {id} not found.");
            return trip;
        }

        public async Task<Trip> CreateTripAsync(CreateTripDTO tripDto)
        {
            if (tripDto is null)
                throw new BadRequestException("Trip object cannot be null.");

            User user = await _userRepository.GetUserByIdAsync(tripDto.DriverId) ?? throw new NotFoundException($"User with ID {tripDto.DriverId} not found.");

            Trip trip = ObjectUpdater.MapObject<Trip>(tripDto);

            await _tripRepository.CreateTripAsync(trip);

            return trip;
        }

        public async Task<Trip> UpdateTripAsync(int id, UpdateTripDTO tripDto)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Trip trip = await GetTripByIdAsync(id) ?? throw new NotFoundException($"Trip with ID {id} not found.");

            ObjectUpdater.UpdateObject<Trip, UpdateTripDTO>(trip, tripDto);

            await _tripRepository.UpdateTripAsync(trip);
            return trip;
        }

        public async Task<bool> DeleteTripAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Trip existingTrip = await GetTripByIdAsync(id) ?? throw new NotFoundException($"Trip with ID {id} not found.");

            await _tripRepository.DeleteTripAsync(id);
            return true;
        }
    }
}
