using Carpool.Domain.Entities;
using Carpool.Infrastructure.Context;
using Carpool.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpool.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly CarpoolDbContext _context;

        public MessageRepository(CarpoolDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetMessagesForTripAsync(int tripId)
        {
            return await _context.Messages
                .Where(m => m.TripId == tripId)
                .ToListAsync();
        }

        public async Task<Message> GetMessageByIdAsync(int messageId)
        {
            return await _context.Messages.FindAsync(messageId);
        }

        public async Task SendMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMessageAsync(Message message)
        {
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message != null)
            {
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
            }
        }
    }
}