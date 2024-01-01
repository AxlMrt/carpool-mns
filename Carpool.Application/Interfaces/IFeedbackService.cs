using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface IFeedbackService
    {
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync();
        Task<Feedback> GetFeedbackByIdAsync(Guid id);
        Task<Feedback> CreateFeedbackAsync(Feedback feedback);
        Task<Feedback> UpdateFeedbackAsync(Guid id, Feedback feedback);
        Task<bool> DeleteFeedbackAsync(Guid id);
    }
}