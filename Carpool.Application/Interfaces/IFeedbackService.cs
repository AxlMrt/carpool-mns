using Carpool.Domain.DTOs;
using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface IFeedbackService
    {
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync();
        Task<Feedback> GetFeedbackByIdAsync(int id);
        Task<IEnumerable<Feedback>> GetFeedbacksByUserIdAsync(int userId);
        Task<Feedback> CreateFeedbackAsync(FeedbackCreateDto feedback);
        Task<Feedback> UpdateFeedbackAsync(int id, FeedbackUpdateDto feedback);
        Task<bool> DeleteFeedbackAsync(int id);
    }
}