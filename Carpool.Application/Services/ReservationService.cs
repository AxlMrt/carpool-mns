using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Application.Utils;
using Carpool.Domain.Common;
using Carpool.Domain.DTO.Notifications;
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
        private readonly ICarRepository _carRepository;
        private readonly INotificationService _notificationService;

        public ReservationService(IReservationRepository reservationRepository, IUserRepository userRepository, ITripRepository tripRepository, ICarRepository carRepository, INotificationService notificationService)
        {
            _reservationRepository = reservationRepository;
            _tripRepository = tripRepository;
            _userRepository = userRepository;
            _carRepository = carRepository;
            _notificationService = notificationService;
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

        public async Task<ReservationDTO> CreateReservationAsync(CreateReservationDTO reservationDto)
        {
            if (reservationDto == null)
                throw new BadRequestException("Reservation object cannot be null.");
            
            User user = await _userRepository.GetUserByIdAsync(reservationDto.UserId) ?? throw new NotFoundException($"User with id {reservationDto.UserId} not found.");
            Trip trip = await _tripRepository.GetTripByIdAsync(reservationDto.TripId) ?? throw new NotFoundException($"Trip with id {reservationDto.TripId} not found.");
            Car car = await _carRepository.GetCarByIdAsync(trip.CarId) ?? throw new NotFoundException($"Trip with id {trip.CarId} not found.");

            if (reservationDto.ReservedSeats <= 0 || car.NumberOfSeats < reservationDto.ReservedSeats)
                throw new BadRequestException("Invalid reserved seats.");

            Reservation reservation = ObjectUpdater.MapObject<Reservation>(reservationDto);

            await _reservationRepository.CreateReservationAsync(reservation);
            user.Reservations.Add(reservation);
            await _userRepository.UpdateUserAsync(user);
            await _notificationService.CreateNotificationAsync(new NotificationCreationDTO
            {
                UserId = trip.DriverId,
                Content = "New reservation.",
                Seen = false,
                Type = NotificationType.NewReservation
            });

            return ObjectUpdater.MapObject<ReservationDTO>(reservation);
        }

        public async Task<ReservationDTO> UpdateReservationAsync(int id, UpdateReservationDTO updateReservationDto)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Reservation reservation = await _reservationRepository.GetReservationByIdAsync(id) ?? throw new NotFoundException($"Reservation with ID {id} not found.");

            ObjectUpdater.UpdateObject(reservation, updateReservationDto);

            if (reservation.ReservedSeats <= 0)
                throw new BadRequestException("Invalid reserved seats.");

            await _reservationRepository.UpdateReservationAsync(reservation);
            await _notificationService.CreateNotificationAsync(new NotificationCreationDTO
            {
                UserId = reservation.UserId,
                Content = "Reservation updated",
                Seen = false,
                Type = NotificationType.ReservationUpdate
            });
            return ObjectUpdater.MapObject<ReservationDTO>(reservation);
        }

        public async Task<bool> DeleteReservationAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Reservation existingReservation = await _reservationRepository.GetReservationByIdAsync(id) ?? throw new NotFoundException($"Reservation with ID {id} not found.");
            User user = await _userRepository.GetUserByIdAsync(existingReservation.UserId) ?? throw new NotFoundException($"User with reservation ID {id} not found.");

            user.Reservations.Remove(existingReservation);
            await _reservationRepository.DeleteReservationAsync(id);
            
            return true;
        }
    }
}
