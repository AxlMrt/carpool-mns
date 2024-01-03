using Carpool.Domain.DTOs;
using Carpool.Domain.Entities;

namespace Carpool.Domain.Interfaces
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task<Address> GetAddressByIdAsync(int id);
        Task CreateAddressAsync(AddressCreateDto addressDto, User user);
        Task UpdateAddressAsync(AddressUpdateDto addressDto);
        Task DeleteAddressAsync(int id);
    }
}