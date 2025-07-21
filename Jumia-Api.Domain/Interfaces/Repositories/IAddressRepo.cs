using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Jumia_Api.Domain.Interfaces.Repositories
{
   public interface IAddressRepo :IGenericRepo<Address>
   {
        // Retrieves all addresses of all users
        public Task<IEnumerable<Address?>> GetAllAddressesOfAllUsersAsync();


        // Retrieves all addresses of all a specific user
        public Task<IEnumerable<Address?>> GetAllAddressesByUserIdAsync(string userId);
 

        public Task<Address?> GetAddressByIdAsync(int addressId);

        // ليه هنا مش راضيه تاخد AddressDto
        //public Task<IEnumerable<AddressDto>> GetAllAddressesByUserIdAsync(string userId);

        public Task<Address> AddNewAddressAsync(Address address);


        public Task<bool> UpdateAddressAsync(Address address);

        public Task<bool> DeleteAddressAsync(Address address);

    }
}
