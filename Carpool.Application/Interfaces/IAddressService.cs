using Carpool.Domain.DTO.Address;

namespace Carpool.Application.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDTO>> GetAllAddressesAsync();
        Task<AddressDTO> GetAddressByIdAsync(int id);
        Task<AddressDTO> CreateAddressAsync(CreateAddressDTO addressDto);
        Task<AddressDTO> UpdateAddressAsync(int id, UpdateAddressDTO addressDto);
        Task<bool> DeleteAddressAsync(int id);
    }
}