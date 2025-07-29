using Jumia_Api.Domain.Interfaces.Repositories;
using Jumia_Api.Domain.Models;
using Jumia_Api.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Jumia_Api.Infrastructure.Presistence.Repositories
{
    public class CustomerRepo : GenericRepo<Customer>, ICustomerRepo
    {
        public CustomerRepo(JumiaDbContext context) : base(context)
        {
        }

        public async Task<Customer?> GetCustomerByUserIdAsync(string userId)
        {
            return await _dbSet.Where(c=>c.UserId==userId).FirstOrDefaultAsync();
        }

        public async Task<bool> ToggleBlockStatusAsync(int customerId)
        {
            var customer = await _dbSet.FindAsync(customerId);
            if (customer == null) return false;

            // Toggle the block status
            customer.IsBlocked = !customer.IsBlocked;
            await _context.SaveChangesAsync();

            return customer.IsBlocked;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            var customerRole = "customer";
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == customerRole);

            if (role == null)
            {
                throw new KeyNotFoundException($"Role {customerRole} not found.");
            }

            // Fetch users in the customer role
            var usersInRole = await _context.UserRoles
                .Where(ur => ur.RoleId == role.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            // Fetch customers with their associated user info
            var customers = await _context.Customers
                .Where(c => usersInRole.Contains(c.UserId))  // Match customers with users in the 'customer' role
                .Include(c => c.User) 
               // Include the associated User data (AppUser)
                .ToListAsync();

            return customers;
        }

    }
}
