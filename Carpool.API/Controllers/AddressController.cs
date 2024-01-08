using Microsoft.AspNetCore.Mvc;
using Carpool.Application.Interfaces;
using Carpool.Application.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Carpool.Domain.DTO.Address;

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
        public async Task<ActionResult<IEnumerable<AddressDTO>>> GetAllAddresses()
        {
            IEnumerable<AddressDTO> addresses = await _addressService.GetAllAddressesAsync();
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddressDTO>> GetAddressById(int id)
        {
            AddressDTO address = await _addressService.GetAddressByIdAsync(id);
            return Ok(address);
        }

        [HttpPost]
        public async Task<ActionResult<AddressDTO>> CreateAddress(CreateAddressDTO addressDto)
        {
            AddressDTO address = await _addressService.CreateAddressAsync(addressDto);
            return CreatedAtAction(nameof(GetAddressById), new { id = address.Id }, address);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AddressDTO>> UpdateAddress(int id, UpdateAddressDTO addressDto)
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
