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

        public async Task<Feedback> GetFeedbackByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Empty ID is not allowed.");

            Feedback feedback = await _feedbackRepository.GetFeedbackByIdAsync(id) ?? throw new NotFoundException($"Feedback with ID {id} not found.");
            return feedback;
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new BadRequestException("Empty ID is not allowed.");

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

        public async Task<Feedback> UpdateFeedbackAsync(Guid id, Feedback feedback)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Empty ID is not allowed.");

            Feedback existingFeedback = await _feedbackRepository.GetFeedbackByIdAsync(id) ?? throw new NotFoundException($"Feedback with ID {id} not found.");

            feedback.Id = existingFeedback.Id;
            await _feedbackRepository.UpdateFeedbackAsync(feedback);
            return feedback;
        }

        public async Task<bool> DeleteFeedbackAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Empty ID is not allowed.");

            Feedback existingFeedback = await _feedbackRepository.GetFeedbackByIdAsync(id) ?? throw new NotFoundException($"Feedback with ID {id} not found.");
            await _feedbackRepository.DeleteFeedbackAsync(id);
            return true;
        }
    }
}