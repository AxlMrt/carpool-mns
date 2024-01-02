using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;
using Carpool.Application.Exceptions;

namespace Carpool.API.Controllers
{
    public class FeedbackController : BaseApiController, IExceptionFilter
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetAllFeedbacks()
        {
            IEnumerable<Feedback> feedbacks = await _feedbackService.GetAllFeedbacksAsync();
            return Ok(feedbacks);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFeedback(Guid id)
        {
            await _feedbackService.DeleteFeedbackAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedbackById(Guid id)
        {
            Feedback feedback = await _feedbackService.GetFeedbackByIdAsync(id);
            return Ok(feedback);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacksByUserId(Guid userId)
        {
            IEnumerable<Feedback> feedbacks = await _feedbackService.GetFeedbacksByUserIdAsync(userId);
            return Ok(feedbacks);
        }
        
        [HttpPost]
        public async Task<ActionResult<Feedback>> AddFeedback(Feedback feedback)
        {
            Feedback createdFeedback = await _feedbackService.CreateFeedbackAsync(feedback);
            return CreatedAtAction(nameof(GetFeedbackById), new { id = createdFeedback.Id }, createdFeedback);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Feedback>> UpdateFeedback(Guid id, Feedback feedback)
        {
            await _feedbackService.UpdateFeedbackAsync(id, feedback);
            return NoContent();
        }

        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
            int statusCode = 500;

            if (exception is NotFoundException)
            {
                statusCode = 404;
            }
            else if (exception is BadRequestException || exception is ArgumentException)
            {
                statusCode = 400;
            }

            context.Result = new ObjectResult(exception.Message)
            {
                StatusCode = statusCode
            };
        }
    }
}
