using Carpool.Domain.DTOs;

namespace Carpool.Domain.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDTO>> GetMessagesForTripAsync(int tripId);
        Task<MessageDTO> GetMessageByIdAsync(int id);
        Task SendMessageAsync(MessageDTO message);
        Task UpdateMessageAsync(MessageDTO message);
        Task DeleteMessageAsync(int messageId);
    }
}