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
            return await _feedbackRepository.GetAllFeedbacksAsync();
        }

        public async Task<Feedback> GetFeedbackByIdAsync(Guid id)
        {
            return await _feedbackRepository.GetFeedbackByIdAsync(id);
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByUserIdAsync(Guid userId)
        {
            return await _feedbackRepository.GetFeedbacksByUserIdAsync(userId);
        }

        public async Task<Feedback> CreateFeedbackAsync(Feedback feedback)
        {
            await _feedbackRepository.CreateFeedbackAsync(feedback);
            return feedback;
        }

        public async Task<Feedback> UpdateFeedbackAsync(Guid id, Feedback feedback)
        {
            var existingFeedback = await _feedbackRepository.GetFeedbackByIdAsync(id);
            if (existingFeedback == null)
                return null;

            feedback.Id = existingFeedback.Id;
            await _feedbackRepository.UpdateFeedbackAsync(feedback);
            return feedback;
        }

        public async Task<bool> DeleteFeedbackAsync(Guid id)
        {
            var existingFeedback = await _feedbackRepository.GetFeedbackByIdAsync(id);
            if (existingFeedback == null)
                return false;

            await _feedbackRepository.DeleteFeedbackAsync(id);
            return true;
        }
    }
}