using AutoMapper;
using Jumia_Api.Application.Dtos.ProductDtos;
using Jumia_Api.Application.Interfaces;
using Jumia_Api.Domain.Interfaces.UnitOfWork;
using Jumia_Api.Domain.Models;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Errors.Model;

namespace Jumia_Api.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task CreateProduct(AddProductDto product)
        {
            if (product == null)
            {
                _logger.LogError("CreateProduct called with null product.");
                return;
            }
            var entity = _mapper.Map<Product>(product);

            await _unitOfWork.ProductRepo.AddAsync(entity);

            await _unitOfWork.SaveChangesAsync();
        }

        public  async Task DeleteProduct(int productId)
        {
            await _unitOfWork.ProductRepo.Delete(productId);
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAvailableProductsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            var product = await _unitOfWork.ProductRepo.GetByIdAsync(productId);
            if (product == null)
            {
                _logger.LogWarning($"Product with ID {productId} not found.");
                return null; // or throw an exception if preferred
            }

            return _mapper.Map<ProductDto>(product);

        }

        public async Task<IEnumerable<Product>> GetProductsByCategoriesAsync(ProductFilterRequestDto productFilterRequestDto)
        {
            if (productFilterRequestDto == null || !productFilterRequestDto.CategoryIds.Any())
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

        public async Task UpdateProduct(UpdateProductDto dto)
        {
            var product = await _unitOfWork.ProductRepo.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {dto.ProductId} not found.");

            var entity = _mapper.Map<Product>(dto);
            _unitOfWork.ProductRepo.Update(entity);
            await _unitOfWork.SaveChangesAsync();

        }



    }
}





