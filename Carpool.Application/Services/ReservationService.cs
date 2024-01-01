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
            return await _reservationRepository.GetAllReservationsAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(Guid id)
        {
            return await _reservationRepository.GetReservationByIdAsync(id);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(Guid userId)
        {
            return await _reservationRepository.GetReservationsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByTripIdAsync(Guid tripId)
        {
            return await _reservationRepository.GetReservationsByTripIdAsync(tripId);
        }

        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            await _reservationRepository.CreateReservationAsync(reservation);
            return reservation;
        }

        public async Task<Reservation> UpdateReservationAsync(Guid id, Reservation reservation)
        {
            var existingReservation = await _reservationRepository.GetReservationByIdAsync(id);
            if (existingReservation == null)
                return null;

            reservation.Id = existingReservation.Id;
            await _reservationRepository.UpdateReservationAsync(reservation);
            return reservation;
        }

        public async Task<bool> DeleteReservationAsync(Guid id)
        {
            var existingReservation = await _reservationRepository.GetReservationByIdAsync(id);
            if (existingReservation == null)
                return false;

            await _reservationRepository.DeleteReservationAsync(id);
            return true;
        }
    }
}