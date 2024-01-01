using Microsoft.AspNetCore.Mvc;
using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;

namespace Carpool.API.Controllers
{
    public class FeedbackController : BaseApiController
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedbackById(Guid id)
        {
            try
            {
                var feedback = await _feedbackService.GetFeedbackByIdAsync(id);
                if (feedback == null)
                    return NotFound();

                return Ok(feedback);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacksByUserId(Guid userId)
        {
            try
            {
                var feedbacks = await _feedbackService.GetFeedbacksByUserIdAsync(userId);
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<Feedback>> AddFeedback(Feedback feedback)
        {
            try
            {
                var createdFeedback = await _feedbackService.CreateFeedbackAsync(feedback);
                return CreatedAtAction(nameof(GetFeedbackById), new { id = createdFeedback.Id }, createdFeedback);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Feedback>> UpdateFeedback(Guid id, Feedback feedback)
        {
            try
            {
                if (id != feedback.Id)
                    return BadRequest();

                await _feedbackService.UpdateFeedbackAsync(id, feedback);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}
