using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Application.Utils;
using Carpool.Domain.DTOs.Address;
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

        public async Task<IEnumerable<AddressDTO>> GetAllAddressesAsync()
        {
            IEnumerable<Address> addresses = await _addressRepository.GetAllAddressesAsync();

            if (addresses is null || !addresses.Any())
                throw new NotFoundException("No addresses found in database.");

            return addresses.Select(u => ObjectUpdater.MapObject<AddressDTO>(u));
        }

        public async Task<AddressDTO> GetAddressByIdAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Address address = await _addressRepository.GetAddressByIdAsync(id) ?? throw new NotFoundException($"Address with ID {id} not found.");
            return ObjectUpdater.MapObject<AddressDTO>(address);
        }

        public async Task<AddressDTO> CreateAddressAsync(CreateAddressDTO addressDto)
        {
            if (addressDto is null)
                throw new BadRequestException("Address object cannot be null.");

            User user = await _userRepository.GetUserByIdAsync(addressDto.UserId) ?? throw new NotFoundException($"User with ID {addressDto.UserId} not found.");
            Address address = ObjectUpdater.MapObject<Address>(addressDto);

            string validationResult = ValidationUtils.IsValidAddress(address);
            if (validationResult != "Valid")
                throw new BadRequestException(validationResult);

            await _addressRepository.CreateAddressAsync(address);

            user.Addresses.Add(address);

            await _userRepository.UpdateUserAsync(user);

            return ObjectUpdater.MapObject<AddressDTO>(address);
        }

        public async Task<AddressDTO> UpdateAddressAsync(int id, UpdateAddressDTO addressDto)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Address address = await _addressRepository.GetAddressByIdAsync(id) ?? throw new NotFoundException($"Address with ID {id} not found.");

            ObjectUpdater.UpdateObject<Address, UpdateAddressDTO>(address, addressDto);

            string validationResult = ValidationUtils.IsValidAddress(address);
            if (validationResult != "Valid")
                throw new BadRequestException(validationResult);

            await _addressRepository.UpdateAddressAsync(address);
            return ObjectUpdater.MapObject<AddressDTO>(address);
        }

        public async Task<bool> DeleteAddressAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ID.");

            Address address = await _addressRepository.GetAddressByIdAsync(id) ?? throw new NotFoundException($"Address with ID {id} not found.");
            await _addressRepository.DeleteAddressAsync(id);
            return true;
        }
    }
}
