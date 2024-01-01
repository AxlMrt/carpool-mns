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
            var cars = await _carService.GetCarsByUserIdAsync(userId);
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCarById(Guid id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
                return NotFound();

            return Ok(car);
        }

        [HttpPost]
        public async Task<ActionResult<Car>> AddCar(Car car)
        {
            var createdCar = await _carService.CreateCarAsync(car);
            return CreatedAtAction(nameof(GetCarById), new { id = createdCar.Id }, createdCar);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Car>> UpdateCar(Guid id, Car car)
        {
            if (id != car.Id)
                return BadRequest();

            var existingCar = await _carService.GetCarByIdAsync(id);
            if (existingCar == null)
                return NotFound();

            await _carService.UpdateCarAsync(id, car);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveCar(Guid id)
        {
            var existingCar = await _carService.GetCarByIdAsync(id);
            if (existingCar == null)
                return NotFound();

            await _carService.DeleteCarAsync(id);
            return NoContent();
        }

    }
}