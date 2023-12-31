using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;
using Carpool.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Carpool.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly CarpoolDbContext _context;

        public ReservationRepository(CarpoolDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return await _context.Reservations.ToListAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            return await _context.Reservations.FindAsync(id);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(int userId)
        {
            return await _context.Reservations.Where(r => r.User.Id == userId).ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByTripIdAsync(int tripId)
        {
            return await _context.Reservations.Where(r => r.Trip.Id == tripId).ToListAsync();
        }

        public async Task CreateReservationAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReservationAsync(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReservationAsync(int id)
        {
            var reservationToDelete = await GetReservationByIdAsync(id);
            if (reservationToDelete != null)
            {
                _context.Reservations.Remove(reservationToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}