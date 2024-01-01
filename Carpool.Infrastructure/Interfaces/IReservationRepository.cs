using Carpool.Domain.Entities;

namespace Carpool.Domain.Interfaces
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(Guid id);
        Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(Guid userId);
        Task<IEnumerable<Reservation>> GetReservationsByTripIdAsync(Guid tripId);
        Task CreateReservationAsync(Reservation reservation);
        Task UpdateReservationAsync(Reservation reservation);
        Task DeleteReservationAsync(Guid id);
    }
}