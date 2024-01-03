using Carpool.Domain.DTOs;
using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task<Address> GetAddressByIdAsync(int id);
        Task<AddressCreateDto> CreateAddressAsync(AddressCreateDto addressDto);
        Task<AddressUpdateDto> UpdateAddressAsync(int id, AddressUpdateDto addressDto);
        Task<bool> DeleteAddressAsync(int id);
    }
}