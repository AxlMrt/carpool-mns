using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Application.Utils;
using Carpool.Domain.DTO.Notifications;
using Carpool.Domain.DTOs.Feedback;
using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;
using Carpool.Infrastructure.Interfaces;

namespace Carpool.Application.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly ITripRepository _tripRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository, IUserRepository userRepository, INotificationService notificationService, ITripRepository tripRepository)
        {
            _feedbackRepository = feedbackRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _tripRepository = tripRepository;
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            IEnumerable<Feedback> feedbacks = await _feedbackRepository.GetAllFeedbacksAsync();

            if (feedbacks == null || !feedbacks.Any())
                throw new NotFoundException("No feedbacks found in the database.");

            return feedbacks;
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Feedback feedback = await _feedbackRepository.GetFeedbackByIdAsync(id) ?? throw new NotFoundException($"Feedback with ID {id} not found.");
            return feedback;
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new BadRequestException("Invalid ID.");

            IEnumerable<Feedback> feedbacks = await _feedbackRepository.GetFeedbacksByUserIdAsync(userId) ?? throw new NotFoundException($"Feedbacks with user ID {userId} not found.");
            return feedbacks;
        }

        public async Task<Feedback> CreateFeedbackAsync(CreateFeedbackDTO feedbackDto)
        {
            if (feedbackDto is null)
                throw new BadRequestException("Feedback object cannot be null.");

            User user = await _userRepository.GetUserByIdAsync(feedbackDto.UserId) ?? throw new NotFoundException($"User with ID {feedbackDto.UserId} not found.");
            Trip trip = await _tripRepository.GetTripByIdAsync(feedbackDto.TripId) ?? throw new BadRequestException($"Trip with ID ${feedbackDto.TripId} doesn't exist.");
            Feedback feedback = ObjectUpdater.MapObject<Feedback>(feedbackDto);

            string validationResult = ValidationUtils.IsValidFeedback(feedback);
            if (validationResult != "Valid")
                throw new BadRequestException(validationResult);
        
            await _feedbackRepository.CreateFeedbackAsync(feedback);

            user.FeedbacksGiven.Add(feedback);

            await _userRepository.UpdateUserAsync(user);

            await _notificationService.CreateNotificationAsync(new NotificationCreationDTO
            {
                UserId = trip.DriverId,
                Content = "New feedback added",
                Seen = false,
                Type = NotificationType.FeedbackCreated
            });

            return feedback;
        }

        public async Task<Feedback> UpdateFeedbackAsync(int id, UpdateFeedbackDTO feedbackDto)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Feedback feedback = await _feedbackRepository.GetFeedbackByIdAsync(id) ?? throw new NotFoundException($"Feedback with ID {id} not found.");
    
            ObjectUpdater.UpdateObject<Feedback, UpdateFeedbackDTO>(feedback, feedbackDto);

            string validationResult = ValidationUtils.IsValidFeedback(feedback);
            if (validationResult != "Valid")
                throw new BadRequestException(validationResult);

            await _feedbackRepository.UpdateFeedbackAsync(feedback);

            await _notificationService.CreateNotificationAsync(new NotificationCreationDTO
            {
                UserId = feedback.UserId,
                Content = "Feedback updated",
                Seen = false,
                Type = NotificationType.FeedbackUpdated
            });

            return feedback;
        }

        public async Task<bool> DeleteFeedbackAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Feedback existingFeedback = await _feedbackRepository.GetFeedbackByIdAsync(id) ?? throw new NotFoundException($"Feedback with ID {id} not found.");
            User user = await _userRepository.GetUserByIdAsync(existingFeedback.UserId) ?? throw new NotFoundException($"User with feedback ID {id} not found.");

            await _feedbackRepository.DeleteFeedbackAsync(id);
            user.FeedbacksGiven.Remove(existingFeedback);

            return true;
        }
    }
}
