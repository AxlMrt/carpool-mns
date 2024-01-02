using Microsoft.AspNetCore.Mvc;
using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using Carpool.Application.Exceptions;

namespace Carpool.API.Controllers
{
    public class ReservationController : BaseApiController, IExceptionFilter
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservations()
        {
            IEnumerable<Reservation> reservations = await _reservationService.GetAllReservationsAsync();
            return Ok(reservations);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservationsByUserId(Guid userId)
        {
            IEnumerable<Reservation> reservations = await _reservationService.GetReservationsByUserIdAsync(userId);
            return Ok(reservations);
        }

        [HttpGet("trip/{tripId}")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservationsByTripId(Guid tripId)
        {
            IEnumerable<Reservation> reservations = await _reservationService.GetReservationsByTripIdAsync(tripId);
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservationById(Guid id)
        {
            Reservation reservation = await _reservationService.GetReservationByIdAsync(id);
            return Ok(reservation);
        }

        [HttpPost("create")]
        public async Task<ActionResult<Reservation>> CreateReservation(Reservation reservation)
        {
            Reservation createdReservation = await _reservationService.CreateReservationAsync(reservation);
            return CreatedAtAction(nameof(GetReservationById), new { id = createdReservation.Id }, createdReservation);
        }

        [HttpDelete("cancel/{id}")]
        public async Task<ActionResult> CancelReservation(Guid id)
        {
            await _reservationService.DeleteReservationAsync(id);
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
