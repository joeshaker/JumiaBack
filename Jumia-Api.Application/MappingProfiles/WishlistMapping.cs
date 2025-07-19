using AutoMapper;
using Jumia_Api.Application.Dtos.WishlistDtos;
using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.MappingProfiles
{
    public class WishlistMapping : Profile
    {
        public WishlistMapping()
        {
            // AddToWishlistDto → WishlistItem (for creating new items)
            CreateMap<AddToWishlistDto, WishlistItem>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // WishlistItem → WishlistItemDto
            CreateMap<WishlistItem, WishlistItemDto>()
                .ForMember(dest => dest.WishlistItemId, opt => opt.MapFrom(src => src.WishlistItemId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.Product.MainImageUrl))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.BasePrice))
                .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src =>  src.Product.DiscountPercentage));

            // Wishlist → WishlistDto
            CreateMap<Wishlist, WishlistDto>()
                .ForMember(dest => dest.wishlistId, opt => opt.MapFrom(src => src.WishlistId))
                .ForMember(dest => dest.customerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.WishlistItems, opt => opt.MapFrom(src => src.WishlistItems))
                .ForMember(dest => dest.totalQuantity, opt => opt.MapFrom(src => src.WishlistItems.Count));
        }
    }
}

