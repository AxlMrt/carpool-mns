using Carpool.Application.Exceptions;
using Carpool.Application.Utils;
using Carpool.Domain.DTOs;
using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;
using Carpool.Infrastructure.Interfaces;

namespace Carpool.Domain.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITripRepository _tripRepository;

        public MessageService(IMessageRepository messageRepository, IUserRepository userRepository, ITripRepository tripRepository)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _tripRepository = tripRepository;
        }

        public async Task<IEnumerable<MessageDTO>> GetMessagesForTripAsync(int tripId)
        {
            
            IEnumerable<Message> messages = await _messageRepository.GetMessagesForTripAsync(tripId);
    
            if (messages == null || !messages.Any())
                throw new NotFoundException("No messages found in the database.");

            return messages.Select(u => ObjectUpdater.MapObject<MessageDTO>(u));
        }

        public async Task SendMessageAsync(MessageDTO messageDto)
        {
            if (messageDto is null)
                throw new BadRequestException("Message object cannot be empty.");

            User sender = await _userRepository.GetUserByIdAsync(messageDto.SenderId) ?? throw new NotFoundException($"User with ID {messageDto.SenderId} not found.");
            User recipient = await _userRepository.GetUserByIdAsync(messageDto.RecipientId) ?? throw new NotFoundException($"User with ID {messageDto.RecipientId} not found.");
            Trip trip = await _tripRepository.GetTripByIdAsync(messageDto.TripId) ?? throw new NotFoundException($"Trip with ID {messageDto.TripId} not found.");

            var message = ObjectUpdater.MapObject<Message>(messageDto);

            await _messageRepository.SendMessageAsync(message);
        }

        public async Task UpdateMessageAsync(MessageDTO messageDto)
        {
            if (messageDto is null)
                throw new BadRequestException("Message object cannot be empty.");

            var existingMessage = await _messageRepository.GetMessageByIdAsync(messageDto.Id) ?? throw new NotFoundException($"Message with ID {messageDto.Id} not found.");

            User sender = await _userRepository.GetUserByIdAsync(messageDto.SenderId) ?? throw new NotFoundException($"Sender with ID {messageDto.SenderId} not found.");
            User recipient = await _userRepository.GetUserByIdAsync(messageDto.RecipientId) ?? throw new NotFoundException($"Recipient with ID {messageDto.RecipientId} not found.");
            Trip trip = await _tripRepository.GetTripByIdAsync(messageDto.TripId) ?? throw new NotFoundException($"Trip with ID {messageDto.TripId} not found.");

            if (existingMessage.SenderId != messageDto.SenderId || existingMessage.RecipientId != messageDto.RecipientId || existingMessage.TripId != messageDto.TripId)
                throw new BadRequestException("Cannot change message sender, recipient, or trip.");

            var messageEntity = ObjectUpdater.MapObject<Message>(messageDto);
            await _messageRepository.UpdateMessageAsync(messageEntity);
        }


        public async Task DeleteMessageAsync(int messageId)
        {
            await _messageRepository.DeleteMessageAsync(messageId);
        }
    }
}