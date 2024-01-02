using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;

namespace Carpool.Application.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
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

        public async Task<Feedback> CreateFeedbackAsync(Feedback feedback)
        {
            if (feedback is null)
                throw new BadRequestException("Feedback object cannot be null.");

            await _feedbackRepository.CreateFeedbackAsync(feedback);
            return feedback;
        }

        public async Task<Feedback> UpdateFeedbackAsync(int id, Feedback feedback)
        {
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");

            Feedback existingFeedback = await _feedbackRepository.GetFeedbackByIdAsync(id) ?? throw new NotFoundException($"Feedback with ID {id} not found.");

            feedback.Id = existingFeedback.Id;
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