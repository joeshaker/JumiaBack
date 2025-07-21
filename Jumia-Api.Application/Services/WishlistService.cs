using AutoMapper;
using Jumia_Api.Application.Dtos.WishlistDtos;
using Jumia_Api.Application.Interfaces;
using Jumia_Api.Domain.Interfaces.UnitOfWork;
using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WishlistService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<WishlistDto> GetWishlistAsync(int customerId)
        {
            var wishlist = await _unitOfWork.WishlistRepo.GetCustomerWishlistAsync(customerId);

            if (wishlist == null)
            {
                wishlist = new Wishlist { CustomerId = customerId };
                await _unitOfWork.WishlistRepo.AddAsync(wishlist);
                await _unitOfWork.SaveChangesAsync();
            }

            return _mapper.Map<WishlistDto>(wishlist);
        }

        public async Task AddItemAsync(int customerId, int productId)
        {
            var wishlist = await _unitOfWork.WishlistRepo.GetCustomerWishlistAsync(customerId)
                          ?? new Wishlist { CustomerId = customerId };

            var product = await _unitOfWork.ProductRepo.GetByIdAsync(productId);
            if (product == null || !product.IsAvailable)
                throw new Exception("Product not available.");

            var existingItem = await _unitOfWork.WishlistItemRepo
                .GetWishlistItemAsync(wishlist.WishlistId, productId);

            if (existingItem != null)
                throw new Exception("Item already in wishlist.");

            var newItem = new WishlistItem
            {
                ProductId = productId,
                WishlistId = wishlist.WishlistId
            };

            wishlist.WishlistItems.Add(newItem);

            if (wishlist.WishlistId == 0)
                await _unitOfWork.WishlistRepo.AddAsync(wishlist);
            else
                _unitOfWork.WishlistRepo.Update(wishlist);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(int customerId, int wishlistItemId)
        {
            var item = await _unitOfWork.WishlistItemRepo.GetByIdAsync(wishlistItemId);
            if (item == null)
                throw new Exception("Wishlist item not found.");

            await _unitOfWork.WishlistItemRepo.Delete(wishlistItemId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ClearWishlistAsync(int customerId)
        {
            await _unitOfWork.WishlistRepo.ClearWishlistAsync(customerId);
            await _unitOfWork.SaveChangesAsync();
        }


    }
}
