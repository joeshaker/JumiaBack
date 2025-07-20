using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Interfaces.Repositories
{
    public interface IWishlistItemRepo : IGenericRepo<WishlistItem>
    {
        public Task<WishlistItem?> GetWishlistItemAsync(int wishlistId, int productId);
    }
}
