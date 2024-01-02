using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task<Address> GetAddressByIdAsync(int id);
        Task<Address> CreateAddressAsync(Address address);
        Task<Address> UpdateAddressAsync(int id, Address address);
        Task<bool> DeleteAddressAsync(int id);
    }
}