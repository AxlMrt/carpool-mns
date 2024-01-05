using Carpool.Domain.DTO.Reservation;

namespace Carpool.Application.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationDTO>> GetAllReservationsAsync();
        Task<ReservationDTO> GetReservationByIdAsync(int id);
        Task<IEnumerable<ReservationDTO>> GetReservationsByUserIdAsync(int userId);
        Task<IEnumerable<ReservationDTO>> GetReservationsByTripIdAsync(int tripId);
        Task<ReservationDTO> CreateReservationAsync(CreateReservationDTO reservation);
        Task<ReservationDTO> UpdateReservationAsync(int id, UpdateReservationDTO reservation);
        Task<bool> DeleteReservationAsync(int id);
    }
}