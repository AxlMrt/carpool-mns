using Microsoft.AspNetCore.Mvc;
using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;

namespace Carpool.API.Controllers
{
    public class TripController : BaseApiController
    {
        private readonly ITripService _tripService;

        public TripController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Trip>> GetTripById(Guid id)
        {
            try
            {
                var trip = await _tripService.GetTripByIdAsync(id);
                if (trip == null)
                    return NotFound();

                return Ok(trip);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Trip>> CreateTrip(Trip trip)
        {
            try
            {
                var createdTrip = await _tripService.CreateTripAsync(trip);
                return CreatedAtAction(nameof(GetTripById), new { id = createdTrip.Id }, createdTrip);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Trip>> UpdateTrip(Guid id, Trip trip)
        {
            try
            {
                var updatedTrip = await _tripService.UpdateTripAsync(id, trip);
                if (updatedTrip == null)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTrip(Guid id)
        {
            try
            {
                var deleted = await _tripService.DeleteTripAsync(id);
                if (!deleted)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trip>>> GetAllTrips()
        {
            try
            {
                var trips = await _tripService.GetAllTripsAsync();
                return Ok(trips);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
