using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Application.Utils;
using Carpool.Domain.DTOs;
using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;
using Carpool.Infrastructure.Interfaces;

namespace Carpool.Application.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserRepository _userRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository, IUserRepository userRepository)
        {
            _feedbackRepository = feedbackRepository;
            _userRepository = userRepository;
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
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");

            Feedback feedback = await _feedbackRepository.GetFeedbackByIdAsync(id) ?? throw new NotFoundException($"Feedback with ID {id} not found.");
            return feedback;
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByUserIdAsync(int userId)
        {
            if (userId < 0)
                throw new BadRequestException("ID cannot be negative.");

            IEnumerable<Feedback> feedback = await _feedbackRepository.GetFeedbacksByUserIdAsync(userId) ?? throw new NotFoundException($"Feedbacks with user ID {userId} not found.");
            return feedback;
        }

        public async Task<Feedback> CreateFeedbackAsync(FeedbackCreateDto feedbackDto)
        {
            if (feedbackDto is null)
                throw new BadRequestException("Feedback object cannot be null.");
            
            User user = await _userRepository.GetUserByIdAsync(feedbackDto.UserId) ?? throw new NotFoundException($"User with ID {feedbackDto.UserId} not found.");

            Feedback feedback = ObjectUpdater.MapDtoToObject<Feedback>(feedbackDto);
        
            await _feedbackRepository.CreateFeedbackAsync(feedback);

            user.FeedbacksGiven.Add(feedback);

            await _userRepository.UpdateUserAsync(user);

            return feedback;
        }

        public async Task<Feedback> UpdateFeedbackAsync(int id, FeedbackUpdateDto feedbackDto)
        {
            if (id <= 0 || id != feedbackDto.Id)
                throw new BadRequestException("Invalid ID.");

            Feedback feedback = await _feedbackRepository.GetFeedbackByIdAsync(feedbackDto.Id) ?? throw new NotFoundException($"Car with ID {id} not found.");
    
            ObjectUpdater.UpdateObject<Feedback, FeedbackUpdateDto>(feedback, feedbackDto);

            await _feedbackRepository.UpdateFeedbackAsync(feedback);
            return feedback;
        }

        public async Task<bool> DeleteFeedbackAsync(int id)
        {
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");

            Feedback existingFeedback = await _feedbackRepository.GetFeedbackByIdAsync(id) ?? throw new NotFoundException($"Feedback with ID {id} not found.");
            await _feedbackRepository.DeleteFeedbackAsync(id);
            return true;
        }
    }
}