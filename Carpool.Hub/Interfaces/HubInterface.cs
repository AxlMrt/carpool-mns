using Carpool.Domain.DTOs;

namespace Carpool.ChatHub
{
    public interface IChatHubService
    {
        Task SendMessage(int senderId, int tripId, string messageContent);
        Task<IEnumerable<MessageDTO>> GetMessagesForTrip(int tripId);
    }
}
