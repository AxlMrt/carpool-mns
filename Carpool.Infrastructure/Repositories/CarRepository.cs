using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;
using Carpool.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Carpool.Infrastructure.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly CarpoolDbContext _context;

        public CarRepository(CarpoolDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<Car> GetCarByIdAsync(Guid id)
        {
            return await _context.Cars.FindAsync(id);
        }

        public async Task CreateCarAsync(Car car)
        {
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCarAsync(Car car)
        {
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCarAsync(Guid id)
        {
            var carToDelete = await GetCarByIdAsync(id);
            if (carToDelete != null)
            {
                _context.Cars.Remove(carToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}