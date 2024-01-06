using Carpool.Application.Exceptions;
using Carpool.Domain.DTOs;
using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Carpool.ChatHub
{
    public class ChatHubService : Hub, IChatHubService
    {
        private readonly IMessageService _messageService;
        private readonly ITripRepository _tripRepository;

        public ChatHubService(IMessageService messageService, ITripRepository tripRepository)
        {
            _messageService = messageService;
            _tripRepository = tripRepository;
        }

        public async Task SendMessage(int senderId, int tripId, string messageContent)
        {
            var sender = Context.User;
            var driverId = await GetDriverIdForTrip(tripId); 

            await _messageService.SendMessageAsync(new MessageDTO
            {
                SenderId = senderId,
                RecipientId = driverId,
                TripId = tripId,
                Content = messageContent
            });

            await Clients.User(driverId.ToString()).SendAsync("ReceiveMessage", new MessageDTO
            {
                SenderId = senderId,
                RecipientId = driverId,
                TripId = tripId,
                Content = messageContent
            });
        }

        public async Task<IEnumerable<MessageDTO>> GetMessagesForTrip(int tripId)
        {
            var messages = await _messageService.GetMessagesForTripAsync(tripId);

            return messages.Select(message => new MessageDTO
            {
                SenderId = message.SenderId,
                RecipientId = message.RecipientId,
                TripId = message.TripId,
                Content = message.Content
            });
        }

        private async Task<int> GetDriverIdForTrip(int tripId)
        {
            Trip trip = await _tripRepository.GetTripByIdAsync(tripId) ?? throw new BadRequestException("Couldn't get trip.");
            return trip.DriverId;
        }
    }
}

