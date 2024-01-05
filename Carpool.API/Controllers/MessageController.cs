using Microsoft.AspNetCore.Mvc;
using Carpool.Domain.DTOs;
using Carpool.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Carpool.Application.Exceptions;

namespace Carpool.API.Controllers
{
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        }

        [HttpGet("trip/{tripId}")]
        public async Task<IActionResult> GetMessagesForTrip(int tripId)
        {
            IEnumerable<MessageDTO> messages = await _messageService.GetMessagesForTripAsync(tripId);
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(MessageDTO message)
        {
            await _messageService.SendMessageAsync(message);
            return Ok("Message sent successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(int id, MessageDTO message)
        {
            if (id != message.Id)
            {
                return BadRequest("Invalid message ID");
            }

            await _messageService.UpdateMessageAsync(message);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            await _messageService.DeleteMessageAsync(id);
            return NoContent();
        }

        [NonAction]
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var statusCode = 500;

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
