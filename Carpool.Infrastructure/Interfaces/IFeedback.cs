using Carpool.Domain.Entities;

namespace Carpool.Domain.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync();
        Task<Feedback> GetFeedbackByIdAsync(Guid id);
        Task<IEnumerable<Feedback>> GetFeedbacksByUserIdAsync(Guid userId);
        Task CreateFeedbackAsync(Feedback feedback);
        Task UpdateFeedbackAsync(Feedback feedback);
        Task DeleteFeedbackAsync(Guid id);
    }
}