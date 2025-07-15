using AutoMapper;
using Jumia_Api.Application.Dtos.ProductDtos;
using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.MappingProfiles
{
    public class ProductsMapping:Profile
    {
        
       public ProductsMapping() {

            CreateMap<Product, ProductDto>()
        .ForMember(dest => dest.AdditionalImageUrls, opt =>
            opt.MapFrom(src => src.ProductImages.OrderBy(i => i.DisplayOrder).Select(i => i.ImageUrl)))
        .ForMember(dest => dest.Attributes, opt =>
            opt.MapFrom(src => src.productAttributeValues));

            CreateMap<ProductVariant, ProductVariantDto>();
            CreateMap<ProductAttributeValue, ProductAttributeValueDto>()
                .ForMember(dest => dest.AttributeName, opt => opt.MapFrom(src => src.ProductAttribute.Name));

            CreateMap<AddProductDto, Product>()
            .ForMember(dest => dest.ProductImages, opt =>
                opt.MapFrom(src => src.AdditionalImageUrls
                    .Select(url => new ProductImage { ImageUrl = url })))
            .ForMember(dest => dest.ProductVariants, opt =>
                opt.MapFrom(src => src.Variants))
            .ForMember(dest => dest.productAttributeValues, opt =>
                opt.MapFrom(src => src.Attributes));

            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.ProductImages, opt =>
                    opt.MapFrom(src => src.AdditionalImageUrls
                    .Select(url => new ProductImage { ImageUrl = url })))
                .ForMember(dest => dest.ProductVariants, opt =>
                    opt.MapFrom(src => src.Variants))
                .ForMember(dest => dest.productAttributeValues, opt =>
                opt.MapFrom(src => src.Attributes));

        }

    }
}
