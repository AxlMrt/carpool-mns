using Microsoft.AspNetCore.Mvc;
using Carpool.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Carpool.Application.Exceptions;
using Carpool.Application.DTO.Trip;
using Microsoft.AspNetCore.Authorization;

namespace Carpool.API.Controllers
{
    [Authorize]
    public class TripController : BaseApiController, IExceptionFilter
    {
        private readonly ITripService _tripService;

        public TripController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripDTO>>> GetAllTrips()
        {
            IEnumerable<TripDTO> trips = await _tripService.GetAllTripsAsync();
            return Ok(trips);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TripDTO>> GetTripById(int id)
        {
            TripDTO trip = await _tripService.GetTripByIdAsync(id);

            return Ok(trip);
        }

        [HttpPost]
        public async Task<ActionResult<TripDTO>> CreateTrip(CreateTripDTO trip)
        {
            TripDTO createdTrip = await _tripService.CreateTripAsync(trip);
            return CreatedAtAction(nameof(GetTripById), new { id = createdTrip.Id }, createdTrip);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TripDTO>> UpdateTrip(int id, UpdateTripDTO trip)
        {
            await _tripService.UpdateTripAsync(id, trip);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTrip(int id)
        {
            await _tripService.DeleteTripAsync(id);

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
