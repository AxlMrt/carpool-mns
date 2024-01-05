using Carpool.Domain.DTOs.Feedback;
using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface IFeedbackService
    {
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync();
        Task<Feedback> GetFeedbackByIdAsync(int id);
        Task<IEnumerable<Feedback>> GetFeedbacksByUserIdAsync(int userId);
        Task<Feedback> CreateFeedbackAsync(CreateFeedbackDTO feedback);
        Task<Feedback> UpdateFeedbackAsync(int id, UpdateFeedbackDTO feedback);
        Task<bool> DeleteFeedbackAsync(int id);
    }
}