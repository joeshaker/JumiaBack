using Jumia_Api.Domain.Interfaces.Repositories;
using Jumia_Api.Domain.Models;
using Jumia_Api.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Jumia_Api.Infrastructure.Presistence.Repositories
{
    public class CartRepo : GenericRepo<Cart>, ICartRepo
    {
        public CartRepo(JumiaDbContext context) : base(context)
        {
        }

        public async Task<Cart?> GetCustomerCartAsync(int customerId)
        {
            return await _dbSet
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.ProductVariant)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task ClearCartAsync(int customerId)
        {
            var cart = await GetCustomerCartAsync(customerId);
            if (cart != null)
            {
                cart.CartItems.Clear();
            }

        }
    }
}
