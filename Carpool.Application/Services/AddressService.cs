using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;

namespace Carpool.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync()
        {
            IEnumerable<Address> addresses = await _addressRepository.GetAllAddressesAsync();

            if (addresses is null || !addresses.Any())
                throw new NotFoundException("No addresses found in database.");
            
            return addresses;
        }

        public async Task<Address> GetAddressByIdAsync(int id)
        {
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");
            
            return await _addressRepository.GetAddressByIdAsync(id) ?? throw new NotFoundException($"Address with ID {id} not found.");
        }

        public async Task<Address> CreateAddressAsync(Address address)
        {
            if (address is null)
                throw new BadRequestException("Address object cannot be null.");
            
            await _addressRepository.CreateAddressAsync(address);
            return address;
        }

        public async Task<Address> UpdateAddressAsync(int id, Address address)
        {
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");

            Address existingAddress = await _addressRepository.GetAddressByIdAsync(id) ?? throw new NotFoundException($"Address with ID {id} not found.");

            address.Id = existingAddress.Id;
            await _addressRepository.UpdateAddressAsync(address);
            return address;
        }

        public async Task<bool> DeleteAddressAsync(int id)
        {
            if (id < 0)
                throw new BadRequestException("ID cannot be negative.");
            
            Address existingAddress = await _addressRepository.GetAddressByIdAsync(id) ?? throw new NotFoundException($"Address with ID {id} not found.");
            await _addressRepository.DeleteAddressAsync(id);
            return true;
        }
    }
}