using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Domain.DTOs;
using Carpool.Domain.Entities;
using Carpool.Domain.Interfaces;
using Carpool.Infrastructure.Interfaces;

namespace Carpool.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUserRepository _userRepository;

        public AddressService(IAddressRepository addressRepository, IUserRepository userRepository)
        {
            _addressRepository = addressRepository;
            _userRepository = userRepository;
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

        public async Task<Address> CreateAddressAsync(AddressCreateDto addressDto)
        {
            if (addressDto is null)
                throw new BadRequestException("Address object cannot be null.");
            
            User user = await _userRepository.GetUserByIdAsync(addressDto.UserId) ?? throw new NotFoundException($"User with ID {addressDto.UserId} not found.");

            Address address = new()
            {
                Street = addressDto.Street,
                City = addressDto.City,
                PostalCode = addressDto.PostalCode,
                Country = addressDto.Country,
                Latitude = addressDto.Latitude,
                Longitude = addressDto.Longitude,
                UserId = user.Id
            };

            await _addressRepository.CreateAddressAsync(address);

            user.Addresses.Add(address);

            await _userRepository.UpdateUserAsync(user);

            return address;
        }

        public async Task<Address> UpdateAddressAsync(int id, AddressUpdateDto addressDto)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Address existingAddress = await _addressRepository.GetAddressByIdAsync(id) ?? throw new NotFoundException($"Address with ID {id} not found.");

            if (!string.IsNullOrEmpty(addressDto.Street) && addressDto.Street != existingAddress.Street)
                existingAddress.Street = addressDto.Street;
            if (!string.IsNullOrEmpty(addressDto.City) && addressDto.City != existingAddress.City)
                existingAddress.City = addressDto.City;
            if (!string.IsNullOrEmpty(addressDto.PostalCode) && addressDto.PostalCode != existingAddress.PostalCode)
                existingAddress.PostalCode = addressDto.PostalCode;
            if (!string.IsNullOrEmpty(addressDto.Country) && addressDto.Country != existingAddress.Country)
                existingAddress.Country = addressDto.Country;
            if (addressDto.Latitude != existingAddress.Latitude)
                existingAddress.Latitude = addressDto.Latitude;
            if (addressDto.Longitude != existingAddress.Longitude)
                existingAddress.Longitude = addressDto.Longitude;

            await _addressRepository.UpdateAddressAsync(existingAddress);
            return existingAddress;
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