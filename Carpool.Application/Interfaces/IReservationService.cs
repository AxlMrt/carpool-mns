using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(Guid id);
        Task<Reservation> CreateReservationAsync(Reservation reservation);
        Task<Reservation> UpdateReservationAsync(Guid id, Reservation reservation);
        Task<bool> DeleteReservationAsync(Guid id);
    }
}