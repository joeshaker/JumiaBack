using Jumia_Api.Application.Dtos.ProductDtos;
using Jumia_Api.Application.Interfaces;
using Jumia_Api.Domain.Interfaces.UnitOfWork;
using Jumia_Api.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Jumia_Api.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoriesAsync(ProductFilterRequestDto productFilterRequestDto)
        {
            if(productFilterRequestDto == null || !productFilterRequestDto.CategoryIds.Any())
            {
                _logger.LogWarning("GetProductsByCategoriesAsync called with null or empty categoryIds. Empty product List is returned");
                 return Enumerable.Empty<Product>();
            }
            var allCategoryIds = productFilterRequestDto.CategoryIds.Distinct().ToList();
            _logger.LogInformation($"GetProductsByCategoriesAsync called with {allCategoryIds.Count} category IDs");

            foreach (var categoryId in allCategoryIds)
            {
                allCategoryIds.Add(categoryId); // include given category
                var descendants = await _unitOfWork.CategoryRepo.GetDescendantCategoryIdsAsync(categoryId);
                allCategoryIds.AddRange(descendants);

            }

            allCategoryIds = allCategoryIds.Distinct().ToList();

            return await _unitOfWork.ProductRepo.GetProductsByCategoryIdsAsync(allCategoryIds,
                                                                                productFilterRequestDto.AttributeFilters,
                                                                                productFilterRequestDto.MinPrice,
                                                                                productFilterRequestDto.MaxPrice);
        }


    }
}

