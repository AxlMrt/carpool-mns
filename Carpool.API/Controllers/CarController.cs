using Microsoft.AspNetCore.Mvc;
using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;

namespace Carpool.API.Controllers
{
    [ApiController]
    [Route("api/cars")]
    public class CarController : BaseApiController
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCarsByUserId(Guid userId)
        {
            try
            {
                var cars = await _carService.GetCarsByUserIdAsync(userId);
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCarById(Guid id)
        {
            try
            {
                var car = await _carService.GetCarByIdAsync(id);
                if (car == null)
                    return NotFound();

                return Ok(car);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Car>> AddCar(Car car)
        {
            try
            {
                var createdCar = await _carService.CreateCarAsync(car);
                return CreatedAtAction(nameof(GetCarById), new { id = createdCar.Id }, createdCar);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Car>> UpdateCar(Guid id, Car car)
        {
            try
            {
                if (id != car.Id)
                    return BadRequest();

                await _carService.UpdateCarAsync(id, car);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveCar(Guid id)
        {
            try
            {
                await _carService.DeleteCarAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
