using Jumia_Api.Application.Dtos.AddressDtos;
using Jumia_Api.Domain.Interfaces.Repositories;
using Jumia_Api.Domain.Models;
using Jumia_Api.Infrastructure.Presistence.Context;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;


namespace Jumia_Api.Infrastructure.Presistence.Repositories
{
    public class AddressRepo : GenericRepo<Address>, IAddressRepo
    {
        public AddressRepo(JumiaDbContext context) : base(context)
        {
        }


        // This method retrieves all addresses of all users
        public async Task<IEnumerable<Address?>> GetAllAddressesOfAllUsersAsync()
        {
            var addresses = await _context.Addresses.ToListAsync();
            return addresses; 
        }



        // This method retrieves all addresses associated with a specific user ID
        public async Task<IEnumerable<Address>> GetAllAddressesByUserIdAsync(string userId)
        {
            var addresses = await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();
            return addresses;
            //return await _dbSet.Where(a => a.UserId == userId).ToListAsync();
        }



        // This method retrieves a specific address by its ID
        public Task<Address?> GetAddressByIdAsync(int addressId)
        {
            var address = _context.Addresses.Where(a => a.AddressId == addressId).FirstOrDefaultAsync();
            return address;
        }



        // This method creates a new address and saves it to the database
        public async Task<Address> AddNewAddressAsync(Address address)
        {
            _dbSet.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }




        // This method updates an existing address by its ID
        public async Task<bool> UpdateAddressAsync(Address address)
        {
            _dbSet.Update(address);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }





        // This method deletes an address by its ID
        public async Task<bool> DeleteAddressAsync(Address address)
        {
            _context.Addresses.Remove(address);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        
    }
}
