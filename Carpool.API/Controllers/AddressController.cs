using Microsoft.AspNetCore.Mvc;
using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;
using Carpool.Application.Exceptions;

namespace Carpool.API.Controllers
{
    public class AddressController : BaseApiController
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAllAddresses()
        {
            try
            {
                IEnumerable<Address> addresses = await _addressService.GetAllAddressesAsync();
                return Ok(addresses);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while fetching all the addresses: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddressById(Guid id)
        {
            try
            {
                var address = await _addressService.GetAddressByIdAsync(id);

                return Ok(address);
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
                return StatusCode(500, $"An error occured while fetching the addresses list: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Address>> CreateAddress(Address address)
        {
            try
            {
                var createdAddress = await _addressService.CreateAddressAsync(address);
                return CreatedAtAction(nameof(GetAddressById), new { id = createdAddress.Id }, createdAddress);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while creating a new address: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Address>> UpdateAddress(Guid id, Address address)
        {
            try
            {
                var updatedAddress = await _addressService.UpdateAddressAsync(id, address);

                return NoContent();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while updating the address: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAddress(Guid id)
        {
            try
            {
                var deleted = await _addressService.DeleteAddressAsync(id);

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
                return StatusCode(500, $"An error occured while removing the address: {ex.Message}");
            }
        }
    }
}
