using Carpool.Domain.Entities;

namespace Carpool.Infrastructure.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetMessagesForTripAsync(int tripId);
        Task<Message> GetMessageByIdAsync(int messageId);
        Task SendMessageAsync(Message message);
        Task UpdateMessageAsync(Message message);
        Task DeleteMessageAsync(int messageId);
    }
}