using Microsoft.AspNetCore.Mvc;
using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;
using Carpool.Application.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Carpool.Domain.Roles;
using Microsoft.AspNetCore.Authorization;
using Carpool.Application.DTO.Car;

namespace Carpool.API.Controllers
{
    [Authorize]
    public class CarController : BaseApiController, IExceptionFilter
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarDTO>>> GetAllCars()
        {
            IEnumerable<Car> cars = await _carService.GetAllCarsAsync();
            return Ok(cars);
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<CarDTO>>> GetCarsByUserId(int userId)
        {
            IEnumerable<Car> cars = await _carService.GetCarsByUserIdAsync(userId);
            return Ok(cars);
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpGet("{id}")]
        public async Task<ActionResult<CarDTO>> GetCarById(int id)
        {
            Car car = await _carService.GetCarByIdAsync(id);
            return Ok(car);
        }

        [HttpPost]
        public async Task<ActionResult<CarDTO>> AddCar(CreateCarDTO car)
        {
            Car createdCar = await _carService.CreateCarAsync(car);
            return CreatedAtAction(nameof(GetCarById), new { id = createdCar.Id }, createdCar);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CarDTO>> UpdateCar(int id, UpdateCarDTO car)
        {
            await _carService.UpdateCarAsync(id, car);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveCar(int id)
        {
            await _carService.DeleteCarAsync(id);
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
