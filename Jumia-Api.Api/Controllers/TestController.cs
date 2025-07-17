using Jumia_Api.Application.Dtos.ProductDtos.Post;
using Jumia_Api.Domain.Models;
using Jumia_Api.Infrastructure.Presistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jumia_Api.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly JumiaDbContext _dbContext;

        public TestController(JumiaDbContext dbContext)
        {
           _dbContext = dbContext;
        }

        [HttpPost("test")]
        public async Task<IActionResult> CreateProductAsync(AddProductDto request)
        {
            // 🪜 Create base product
            var product = new Product
            {
                SellerId = request.SellerId,
                CategoryId = request.CategoryId,
                Name = request.Name,
                Description = request.Description,
                BasePrice = request.BasePrice,
                DiscountPercentage = 0, // No discount on base
                MainImageUrl = request.MainImageUrl,
                IsAvailable = true,
                StockQuantity = request.Variants.Sum(v => v.StockQuantity),
                ApprovalStatus = "pending",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ProductImages = request.AdditionalImageUrls
                    .Select(url => new ProductImage { ImageUrl = url })
                    .ToList(),
                productAttributeValues = new List<ProductAttributeValue>(),
                ProductVariants = new List<ProductVariant>()
            };

            // 🪜 Add product attribute values
            foreach (var attrDto in request.Attributes)
            {
                foreach (var value in attrDto.Values)
                {
                    // Check if attribute already exists for this product
                    if (!_dbContext.ProductAttributeValues.Any(pav =>
                            pav.ProductId == product.ProductId &&
                            pav.ProductAttribute.Name == attrDto.AttributeName &&
                            pav.Value == value))
                    {
                        // Get attribute definition
                        var attribute = await _dbContext.ProductAttributes
                            .FirstOrDefaultAsync(a => a.Name == attrDto.AttributeName && a.CategoryId == request.CategoryId);

                        if (attribute == null)
                            throw new Exception($"Attribute '{attrDto.AttributeName}' not found for this category.");

                        product.productAttributeValues.Add(new ProductAttributeValue
                        {
                            ProductAttribute = attribute,
                            Value = value
                        });
                    }
                }
            }

            // 🪜 Add variants and their attributes
            foreach (var variantDto in request.Variants)
            {
                var variant = new ProductVariant
                {
                    VariantName = variantDto.VariantName,
                    Price = variantDto.Price,
                    DiscountPercentage = variantDto.DiscountPercentage,
                    StockQuantity = variantDto.StockQuantity,
                    SKU = variantDto.SKU,
                    VariantImageUrl = variantDto.VariantImageUrl,
                    IsDefault = variantDto.IsDefault,
                    IsAvailable = variantDto.IsAvailable,
                    Attributes = new List<VariantAttribute>()
                };

                foreach (var va in variantDto.Attributes)
                {
                    variant.Attributes.Add(new VariantAttribute
                    {
                        AttributeName = va.AttributeName,
                        AttributeValue = va.AttributeValue
                    });
                }

                product.ProductVariants.Add(variant);
            }

            // Save all in one go
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return Ok(product.ProductId);
        }

    }
}
