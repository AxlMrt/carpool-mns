using Carpool.Domain.Entities;

namespace Carpool.Application.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task<Address> GetAddressByIdAsync(Guid id);
        Task<Address> CreateAddressAsync(Address address);
        Task<Address> UpdateAddressAsync(Guid id, Address address);
        Task<bool> DeleteAddressAsync(Guid id);
    }
}