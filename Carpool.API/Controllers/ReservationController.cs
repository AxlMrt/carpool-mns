using Microsoft.AspNetCore.Mvc;
using Carpool.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Carpool.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Carpool.Domain.DTO.Reservation;
using Carpool.Domain.Common;

namespace Carpool.API.Controllers
{
    [Authorize]
    public class ReservationController : BaseApiController, IExceptionFilter
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        
        [Authorize(Roles = Roles.Administrator)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetAllReservations()
        {
            IEnumerable<ReservationDTO> reservations = await _reservationService.GetAllReservationsAsync();
            return Ok(reservations);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetReservationsByUserId(int userId)
        {
            IEnumerable<ReservationDTO> reservations = await _reservationService.GetReservationsByUserIdAsync(userId);
            return Ok(reservations);
        }

        [HttpGet("trip/{tripId}")]
        public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetReservationsByTripId(int tripId)
        {
            IEnumerable<ReservationDTO> reservations = await _reservationService.GetReservationsByTripIdAsync(tripId);
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDTO>> GetReservationById(int id)
        {
            ReservationDTO reservation = await _reservationService.GetReservationByIdAsync(id);
            return Ok(reservation);
        }

        [HttpPost]
        public async Task<ActionResult<ReservationDTO>> CreateReservation(CreateReservationDTO reservation)
        {
            ReservationDTO createdReservation = await _reservationService.CreateReservationAsync(reservation);
            return CreatedAtAction(nameof(GetReservationById), new { id = createdReservation.Id }, createdReservation);
        }

        [HttpDelete("cancel/{id}")]
        public async Task<ActionResult> CancelReservation(int id)
        {
            await _reservationService.DeleteReservationAsync(id);
            return NoContent();
        }

        [NonAction]
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
