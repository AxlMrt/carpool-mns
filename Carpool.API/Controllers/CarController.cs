using Microsoft.AspNetCore.Mvc;
using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;
using Carpool.Application.Exceptions;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Carpool.Domain.Roles;

namespace Carpool.API.Controllers
{
    [Authorize]
    public class CarController : BaseApiController
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetAllCars()
        {
            try
            {
                IEnumerable<Car> cars = await _carService.GetAllCarsAsync();
                return Ok(cars);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching all the cars: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCarsByUserId(Guid userId)
        {
            try
            {
                var cars = await _carService.GetCarsByUserIdAsync(userId);
                return Ok(cars);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching user car: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCarById(Guid id)
        {
            try
            {
                var car = await _carService.GetCarByIdAsync(id);

                return Ok(car);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching car: {ex.Message}");
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
                return StatusCode(500, $"An error occurred while creating the car: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Car>> UpdateCar(Guid id, Car car)
        {
            try
            {
                await _carService.UpdateCarAsync(id, car);
                return NoContent();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the car: {ex.Message}");
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
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while removing the car: {ex.Message}");
            }
        }
    }
}
