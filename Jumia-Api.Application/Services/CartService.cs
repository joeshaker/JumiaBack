using AutoMapper;
using Jumia_Api.Application.Dtos.CartDto;
using Jumia_Api.Application.Interfaces;
using Jumia_Api.Domain.Interfaces.Repositories;
using Jumia_Api.Domain.Interfaces.UnitOfWork;
using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IProductRepo productRepo, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CartDto> GetCartAsync(int customerId)
        {
            var cart = await _unitOfWork.CartRepo.GetCustomerCartAsync(customerId);
            if (cart == null)
            {
                // If no cart exists, create a new one
                cart = new Cart { CustomerId = customerId };
                await _unitOfWork.CartRepo.AddAsync(cart);
                await _unitOfWork.SaveChangesAsync();
            }

            return _mapper.Map<CartDto>(cart); // Use AutoMapper or manual mapping
        }

        public async Task AddItemAsync(int customerId, AddToCartDto dto)
        {
            var cart = await _unitOfWork.CartRepo.GetCustomerCartAsync(customerId)
            ?? new Cart { CustomerId = customerId };

            var product = await _unitOfWork.ProductRepo.GetWithVariantsAndAttributesAsync(dto.ProductId);
            if (product == null || !product.IsAvailable)
                throw new Exception("Product not available.");

            if (dto.VariantId.HasValue)
            {
                var variant = product.ProductVariants.FirstOrDefault(v => v.VariantId == dto.VariantId);
                if (variant == null || !variant.IsAvailable)
                    throw new Exception("Variant not available.");
                if (variant.StockQuantity < dto.Quantity)
                    throw new Exception("Insufficient stock for variant.");
            }
            else
            {
                if (product.StockQuantity < dto.Quantity)
                    throw new Exception("Insufficient stock for product.");
            }

            var existingItem = await _unitOfWork.CartItemRepo
                .GetCartItemAsync(cart.CartId, dto.ProductId, dto.VariantId);

            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                var newItem = new CartItem
                {
                    ProductId = dto.ProductId,
                    VariationId = dto.VariantId,
                    Quantity = dto.Quantity,
                    PriceAtAddition = dto.VariantId.HasValue
                        ? product.ProductVariants.First(v => v.VariantId == dto.VariantId).Price
                        : product.BasePrice
                };

                cart.CartItems.Add(newItem);
            }

            if (cart.CartId == 0)
                await _unitOfWork.CartRepo.AddAsync(cart);
            else
                _unitOfWork.CartRepo.Update(cart);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateItemQuantityAsync(int customerId, int cartItemId, int quantity)
        {
            var item = await _unitOfWork.CartItemRepo.GetByIdAsync(cartItemId);
            if (item == null)
                throw new Exception("Cart item not found.");

            if (quantity <= 0)
                await _unitOfWork.CartItemRepo.Delete(item.CartItemId);
            else
                item.Quantity = quantity;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(int customerId, int cartItemId)
        {
            await _unitOfWork.CartItemRepo.Delete(cartItemId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ClearCartAsync(int customerId)
        {
            await _unitOfWork.CartRepo.ClearCartAsync(customerId);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
