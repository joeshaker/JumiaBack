using Jumia_Api.Domain.Interfaces.Repositories;
using Jumia_Api.Domain.Models;
using Jumia_Api.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Infrastructure.Presistence.Repositories
{
    internal class WishlistItemRepo : GenericRepo<WishlistItem> , IWishlistItemRepo
    {
        public WishlistItemRepo(JumiaDbContext context) : base(context)
        {
        }
        public async Task<WishlistItem?> GetWishlistItemAsync(int wishlistId, int productId)
        {
            return await _dbSet.FirstOrDefaultAsync(wi =>
                wi.WishlistId == wishlistId &&
                wi.ProductId == productId);
        }
    }
}
