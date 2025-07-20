using Jumia_Api.Domain.Models;
using Jumia_Api.Infrastructure.Presistence.Context;
using Jumia_Api.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Jumia_Api.Infrastructure.Presistence.Repositories
{
    internal class WishlistRepo : GenericRepo<Wishlist>, IWishlistRepo
    {

        public WishlistRepo(JumiaDbContext context) : base(context)
        {
        }
        public async Task<Wishlist?> GetCustomerWishlistAsync(int customerId)
        {
            return await _dbSet
                .Include(w => w.WishlistItems)
                    .ThenInclude(wi => wi.Product)
                .FirstOrDefaultAsync(w => w.CustomerId == customerId);
        }
        public async Task ClearWishlistAsync(int customerId)
        {
            var wishlist = await GetCustomerWishlistAsync(customerId);
            if (wishlist != null)
            {
                wishlist.WishlistItems.Clear();
            }
        }

    }
}
