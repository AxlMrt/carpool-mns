using Carpool.Domain.DTOs.Address;
using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task<Address> GetAddressByIdAsync(int id);
        Task<Address> CreateAddressAsync(CreateAddressDTO addressDto);
        Task<Address> UpdateAddressAsync(int id, UpdateAddressDTO addressDto);
        Task<bool> DeleteAddressAsync(int id);
    }
}