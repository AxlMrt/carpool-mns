using System.Security.Claims;
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
            ClaimsPrincipal sender = Context.User;
            int driverId = await GetDriverIdForTrip(tripId); 
            
            Trip trip = await _tripRepository.GetTripByIdAsync(tripId);
            await _messageService.SendMessageAsync(new MessageDTO
            {
                Content = messageContent,
                TimeStamp = DateTime.UtcNow,
                SenderId = senderId,
                SenderName = sender.Identity.Name, // Sender's name is retrieved from the authentication context
                RecipientId = driverId,
                TripId = tripId,
                TripTitle = $"Voyage pour {trip.DepartureAddress.City}."
            });

            await Clients.User(driverId.ToString()).SendAsync("ReceiveMessage", new MessageDTO
            {
                Content = messageContent,
                TimeStamp = DateTime.UtcNow,
                SenderId = senderId,
                SenderName = sender.Identity.Name,
                RecipientId = driverId,
                TripId = tripId,
                TripTitle = $"Voyage pour {trip.DepartureAddress.City}."
            });

            await Clients.User(driverId.ToString()).SendAsync("ReceiveNotification", "Vous avez reçu un nouveau message !");
        }

        public async Task SendReplyToMessage(int senderId, int messageId, string messageContent)
        {
            ClaimsPrincipal sender = Context.User;
            MessageDTO originalMessage = await _messageService.GetMessageByIdAsync(messageId);

            if (originalMessage == null || originalMessage.RecipientId != senderId)
                throw new BadRequestException("Invalid message or recipient.");

            await _messageService.SendMessageAsync(new MessageDTO
            {
                Content = messageContent,
                TimeStamp = DateTime.UtcNow,
                SenderId = senderId,
                SenderName = sender.Identity.Name,
                RecipientId = originalMessage.SenderId,
                TripId = originalMessage.TripId,
                TripTitle = $"Voyage pour {originalMessage.TripTitle}."
            });

            await Clients.User(originalMessage.SenderId.ToString()).SendAsync("ReceiveMessage", new MessageDTO
            {
                Content = messageContent,
                TimeStamp = DateTime.UtcNow,
                SenderId = senderId,
                SenderName = sender.Identity.Name,
                RecipientId = originalMessage.SenderId,
                TripId = originalMessage.TripId,
                TripTitle = $"Voyage pour {originalMessage.TripTitle}."
            });

            await Clients.User(originalMessage.SenderId.ToString()).SendAsync("ReceiveNotification", "Vous avez reçu une réponse à votre message !");
        }

        public async Task<IEnumerable<MessageDTO>> GetMessagesForTrip(int tripId)
        {
            IEnumerable<MessageDTO> messages = await _messageService.GetMessagesForTripAsync(tripId);

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

