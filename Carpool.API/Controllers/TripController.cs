
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
            var trip = await _tripService.GetTripByIdAsync(id);
            if (trip == null)
            {
                return NotFound();
            }

            return Ok(trip);
        }

        [HttpPost]
        public async Task<ActionResult<Trip>> CreateTrip(Trip trip)
        {
            var createdTrip = await _tripService.CreateTripAsync(trip);
            return CreatedAtAction(nameof(GetTripById), new { id = createdTrip.Id }, createdTrip);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Trip>> UpdateTrip(Guid id, Trip trip)
        {
            if (id != trip.Id)
                return BadRequest();

            var existingTrip = await _tripService.GetTripByIdAsync(id);
            if (existingTrip == null)
                return NotFound();

            await _tripService.UpdateTripAsync(id, trip);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTrip(Guid id)
        {
            var existingTrip = await _tripService.GetTripByIdAsync(id);
            if (existingTrip == null)
                return NotFound();

            await _tripService.DeleteTripAsync(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trip>>> GetAllTrips()
        {
            var trips = await _tripService.GetAllTripsAsync();
            return Ok(trips);
        }
    }
}
