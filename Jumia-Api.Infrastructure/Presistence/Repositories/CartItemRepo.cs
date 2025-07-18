using Jumia_Api.Domain.Interfaces.Repositories;
using Jumia_Api.Domain.Models;
using Jumia_Api.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Jumia_Api.Infrastructure.Presistence.Repositories
{
    public class CartItemRepo : GenericRepo<CartItem>, ICartItemRepo
    {
        public CartItemRepo(JumiaDbContext context) : base(context)
        {
        }

        public async Task<CartItem?> GetCartItemAsync(int cartId, int productId, int? variantId)
        {
            return await _dbSet.FirstOrDefaultAsync(ci =>
                ci.CartId == cartId &&
                ci.ProductId == productId &&
                ci.VariationId == variantId);
        }

    }
}
