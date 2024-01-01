using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(Guid id);
        Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(Guid userId);
        Task<IEnumerable<Reservation>> GetReservationsByTripIdAsync(Guid tripId);
        Task<Reservation> CreateReservationAsync(Reservation reservation);
        Task<Reservation> UpdateReservationAsync(Guid id, Reservation reservation);
        Task<bool> DeleteReservationAsync(Guid id);
    }
}