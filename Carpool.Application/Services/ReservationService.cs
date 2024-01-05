using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Application.Utils;
using Carpool.Domain.DTO.Reservation;
using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;
using Carpool.Infrastructure.Interfaces;

namespace Carpool.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITripRepository _tripRepository;

        public ReservationService(IReservationRepository reservationRepository, IUserRepository userRepository, ITripRepository tripRepository)
        {
            _reservationRepository = reservationRepository;
            _tripRepository = tripRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ReservationDTO>> GetAllReservationsAsync()
        {
            IEnumerable<Reservation> reservations = await _reservationRepository.GetAllReservationsAsync();

            if (reservations == null || !reservations.Any())
                throw new NotFoundException("No reservations found.");

            return reservations.Select(r => ObjectUpdater.MapObject<ReservationDTO>(r));
        }

        public async Task<ReservationDTO> GetReservationByIdAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Reservation reservation = await _reservationRepository.GetReservationByIdAsync(id) ?? throw new NotFoundException($"Reservation with ID {id} not found.");

            return ObjectUpdater.MapObject<ReservationDTO>(reservation);
        }

        public async Task<IEnumerable<ReservationDTO>> GetReservationsByUserIdAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            IEnumerable<Reservation> reservations = await _reservationRepository.GetReservationsByUserIdAsync(id);

            if (reservations == null || !reservations.Any())
                throw new NotFoundException($"No reservations found for user with ID {id}.");

            return reservations.Select(r => ObjectUpdater.MapObject<ReservationDTO>(r));
        }

        public async Task<IEnumerable<ReservationDTO>> GetReservationsByTripIdAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            var reservations = await _reservationRepository.GetReservationsByTripIdAsync(id);
            if (reservations == null || !reservations.Any())
                throw new NotFoundException($"No reservations found for trip with ID {id}.");

            return reservations.Select(r => ObjectUpdater.MapObject<ReservationDTO>(r));
        }

        public async Task<ReservationDTO> CreateReservationAsync(CreateReservationDTO createReservationDto)
        {
            if (createReservationDto == null)
                throw new BadRequestException("Reservation object cannot be null.");
            
            
            User user = await _userRepository.GetUserByIdAsync(createReservationDto.UserId) ?? throw new NotFoundException($"User with id {createReservationDto.UserId} not found.");
            Trip trip = await _tripRepository.GetTripByIdAsync(createReservationDto.TripId) ?? throw new NotFoundException($"Trip with id {createReservationDto.TripId} not found.");

            if (createReservationDto.ReservedSeats < 0 || trip.Car.NumberOfSeats < createReservationDto.ReservedSeats)
                throw new BadRequestException("Invalid reserved seats.");

            Reservation reservation = ObjectUpdater.MapObject<Reservation>(createReservationDto);
            await _reservationRepository.CreateReservationAsync(reservation);
            return ObjectUpdater.MapObject<ReservationDTO>(reservation);
        }

        public async Task<ReservationDTO> UpdateReservationAsync(int id, UpdateReservationDTO updateReservationDto)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Reservation existingReservation = await _reservationRepository.GetReservationByIdAsync(id) ?? throw new NotFoundException($"Reservation with ID {id} not found.");

            if (updateReservationDto.ReservedSeats <= 0 || existingReservation.ReservedSeats < updateReservationDto.ReservedSeats)
                throw new BadRequestException("Invalid reserved seats.");

            existingReservation.ReservedSeats = updateReservationDto.ReservedSeats;
            existingReservation.Status = updateReservationDto.Status;

            await _reservationRepository.UpdateReservationAsync(existingReservation);
            return ObjectUpdater.MapObject<ReservationDTO>(existingReservation);
        }

        public async Task<bool> DeleteReservationAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Reservation existingReservation = await _reservationRepository.GetReservationByIdAsync(id) ?? throw new NotFoundException($"Reservation with ID {id} not found.");

            await _reservationRepository.DeleteReservationAsync(id);

            return true;
        }
    }
}
