using Microsoft.AspNetCore.Mvc;
using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;
using Carpool.Application.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Carpool.Domain.DTOs;

namespace Carpool.API.Controllers
{
    [Authorize]
    public class AddressController : BaseApiController, IExceptionFilter
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAllAddresses()
        {
            IEnumerable<Address> addresses = await _addressService.GetAllAddressesAsync();
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddressById(int id)
        {
            Address address = await _addressService.GetAddressByIdAsync(id);
            return Ok(address);
        }

        [HttpPost]
        public async Task<ActionResult<AddressCreateDto>> CreateAddress(AddressCreateDto addressDto)
        {
            await _addressService.CreateAddressAsync(addressDto);
            return Ok("Address registered successfully.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AddressUpdateDto>> UpdateAddress(int id, AddressUpdateDto addressDto)
        {
            await _addressService.UpdateAddressAsync(id, addressDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAddress(int id)
        {
            await _addressService.DeleteAddressAsync(id);
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
