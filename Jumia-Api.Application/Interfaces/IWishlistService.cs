using Jumia_Api.Application.Dtos.WishlistDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Interfaces
{
    public interface IWishlistService  // Changed from 'class' to 'interface'
    {
        Task<WishlistDto> GetWishlistAsync(int customerId);
        Task AddItemAsync(int customerId, int productId);
        Task RemoveItemAsync(int customerId, int wishlistItemId);
        Task ClearWishlistAsync(int customerId);
    }
}