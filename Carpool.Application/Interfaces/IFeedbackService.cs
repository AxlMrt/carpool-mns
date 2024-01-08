using Carpool.Domain.DTOs.Feedback;

namespace Carpool.Application.Interfaces
{
    public interface IFeedbackService
    {
        Task<IEnumerable<FeedbackDTO>> GetAllFeedbacksAsync();
        Task<FeedbackDTO> GetFeedbackByIdAsync(int id);
        Task<IEnumerable<FeedbackDTO>> GetFeedbacksByUserIdAsync(int userId);
        Task<FeedbackDTO> CreateFeedbackAsync(CreateFeedbackDTO feedback);
        Task<FeedbackDTO> UpdateFeedbackAsync(int id, UpdateFeedbackDTO feedback);
        Task<bool> DeleteFeedbackAsync(int id);
    }
}