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

        public async Task<Address> GetAddressByIdAsync(Guid id)
        {
            return await _addressRepository.GetAddressByIdAsync(id);
        }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync()
        {
            return await _addressRepository.GetAllAddressesAsync();
        }

        public async Task<Address> CreateAddressAsync(Address address)
        {
            await _addressRepository.CreateAddressAsync(address);
            return address;
        }

        public async Task<Address> UpdateAddressAsync(Guid id, Address address)
        {
            var existingAddress = await _addressRepository.GetAddressByIdAsync(id) ?? throw new NotFoundException($"Address with ID {id} not found.");

            address.Id = existingAddress.Id;
            await _addressRepository.UpdateAddressAsync(address);
            return address;
        }

        public async Task<bool> DeleteAddressAsync(Guid id)
        {
            var existingAddress = await _addressRepository.GetAddressByIdAsync(id) ?? throw new NotFoundException($"Address with ID {id} not found.");
            await _addressRepository.DeleteAddressAsync(id);
            return true;
        }
    }
}