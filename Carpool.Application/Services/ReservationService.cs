using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;

namespace Carpool.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            IEnumerable<Reservation> reservations = await _reservationRepository.GetAllReservationsAsync();

            if (reservations == null || !reservations.Any())
                throw new NotFoundException("No reservations found.");

            return reservations;
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");

            Reservation reservation = await _reservationRepository.GetReservationByIdAsync(id) ?? throw new NotFoundException($"Reservation with ID {id} not found.");
            return reservation;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(int id)
        {
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");

            IEnumerable<Reservation> reservations = await _reservationRepository.GetReservationsByUserIdAsync(id);

            if (reservations == null || !reservations.Any())
                throw new NotFoundException($"No reservations found for user with ID {id}.");

            return reservations;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByTripIdAsync(int id)
        {
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");

            var reservations = await _reservationRepository.GetReservationsByTripIdAsync(id);
            if (reservations == null || !reservations.Any())
                throw new NotFoundException($"No reservations found for trip with ID {id}.");

            return reservations;
        }

        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            if (reservation == null)
                throw new BadRequestException("Reservation object cannot be null.");

            await _reservationRepository.CreateReservationAsync(reservation);
            return reservation;
        }

        public async Task<Reservation> UpdateReservationAsync(int id, Reservation reservation)
        {
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");

            Reservation existingReservation = await GetReservationByIdAsync(id) ?? throw new NotFoundException($"Reservation with ID {id} not found.");

            await _reservationRepository.UpdateReservationAsync(existingReservation);
            return existingReservation;
        }

        public async Task<bool> DeleteReservationAsync(int id)
        {
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");

            Reservation existingReservation = await GetReservationByIdAsync(id) ?? throw new NotFoundException($"Reservation with ID {id} not found.");

            await _reservationRepository.DeleteReservationAsync(id);

            return true;
        }
    }
}
