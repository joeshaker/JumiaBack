using AutoMapper;
using Jumia_Api.Application.Common.Results;
using Jumia_Api.Application.Dtos.ProductDtos;
using Jumia_Api.Application.Dtos.ProductDtos.Get;
using Jumia_Api.Application.Dtos.ProductDtos.Post;
using Jumia_Api.Application.Interfaces;
using Jumia_Api.Domain.Interfaces.UnitOfWork;
using Jumia_Api.Domain.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Errors.Model;

namespace Jumia_Api.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        public ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger, IMapper mapper, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _fileService = fileService;
        }


        public  async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _unitOfWork.ProductRepo.GetByIdAsync(productId);
            if (product == null) { return false; }
            await _unitOfWork.ProductRepo.Delete(productId);
            await _unitOfWork.SaveChangesAsync();
   
            return true;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _unitOfWork.ProductRepo.GetByIdAsync(id);
        }

        public async Task <IEnumerable<ProductDetailsDto>> GetAllProductsWithDetailsAsync()
        {
            //available and not available products
            //done gets the product details by productId(prod, attributes and variants)
            var products = await _unitOfWork.ProductRepo.GetAllWithVariantsAndAttributesAsync();
            if (products == null || !products.Any())
            {
                _logger.LogWarning("No available products found.");
                return Enumerable.Empty<ProductDetailsDto>();
            }
            _logger.LogInformation($"Found {products.Count()} available products.");
            return _mapper.Map<IEnumerable<ProductDetailsDto>>(products);
        }

      
        public async Task<IEnumerable<ProductsUIDto>> GetProductsBySellerIdAsync(int sellerId, string role)
        {
            var products = await _unitOfWork.ProductRepo.GetProductsBySellerId(sellerId);
            if (products == null || !products.Any())
            {
                _logger.LogWarning($"No products found for seller with ID {sellerId}.");
                return Enumerable.Empty<ProductsUIDto>();
            }
            _logger.LogInformation($"Found {products.Count()} products for seller with ID {sellerId}.");
            if (role == "Seller" || role == "Admin")
            {
                return products.Select(p => _mapper.Map<ProductsUIDto>(p)).ToList();
            }
            else
            {
                // For non-seller and non-admin roles, filter out unavailable products
                var availableProducts = products.Where(p => p.IsAvailable);
                return availableProducts.Select(p => _mapper.Map<ProductsUIDto>(p)).ToList();
            }

        }







        //seller & customer & admin
        public async Task<ProductDetailsDto> GetProductDetailsAsync(int productId, string role)
        {
            //done gets the product details by productId(prod, attributes and variants)
            // available and not available products
            var product = await _unitOfWork.ProductRepo.GetWithVariantsAndAttributesAsync(productId);
            if (product == null)
            {
                _logger.LogWarning($"Product with ID {productId} not found.");
                return null; 
            }
            if(!product.IsAvailable && role == "Customer" )
            {
                _logger.LogWarning($"Product with ID {productId} is not available for non-admin users.");
                return null; // or throw an exception if preferred
            }

            return _mapper.Map<ProductDetailsDto>(product);
        }

        public async Task<IEnumerable<ProductsUIDto>> GetAllProductsAsync()
        {
            //done gets the available products
            var products = await _unitOfWork.ProductRepo.GetAllAsync();
            if (products == null || !products.Any())
            {
                _logger.LogWarning("No available products found.");
                return Enumerable.Empty<ProductsUIDto>();
            }
            _logger.LogInformation($"Found {products.Count()} available products.");
            return _mapper.Map<IEnumerable<ProductsUIDto>>(products);
        }




        public async Task<PagedResult<ProductsUIDto>> GetProductsByCategoriesAsync(string role,ProductFilterRequestDto productFilterRequestDto
                                                                                   ,int pageNumber = 1,     
                                                                                    int pageSize = 20)

        {

            if (productFilterRequestDto == null || !productFilterRequestDto.CategoryIds.Any())
            {
                _logger.LogWarning("GetProductsByCategoriesAsync called with null or empty categoryIds. Empty product List is returned");
                return new PagedResult<ProductsUIDto>(null, 0, pageNumber, pageSize);
            }
            var allCategoryIds = productFilterRequestDto.CategoryIds.Distinct().ToList();
            _logger.LogInformation($"GetProductsByCategoriesAsync called with {allCategoryIds.Count} category IDs");

            var additionalCategoryIds = new List<int>();

            foreach (var categoryId in allCategoryIds)
            {
                additionalCategoryIds.Add(categoryId); // include given category
                var descendants = await _unitOfWork.CategoryRepo.GetDescendantCategoryIdsAsync(categoryId);
                additionalCategoryIds.AddRange(descendants);
            }

            allCategoryIds.AddRange(additionalCategoryIds);
            allCategoryIds = allCategoryIds.Distinct().ToList();

            allCategoryIds = allCategoryIds.Distinct().ToList();

            var pagedProducts =  await _unitOfWork.ProductRepo.GetProductsByCategoryIdsAsync(allCategoryIds,
                                                                                productFilterRequestDto.AttributeFilters,
                                                                                productFilterRequestDto.MinPrice,
                                                                                productFilterRequestDto.MaxPrice,
                                                                                pageNumber,
                                                                                pageSize);
            if (pagedProducts == null || !pagedProducts.Items.Any())
            {
                _logger.LogWarning("No products found for the given categories.");
                return new PagedResult<ProductsUIDto>(null, 0,pageNumber,pageSize); 
            }

            _logger.LogInformation($"Found {pagedProducts.TotalCount} products for the given categories.");

            IEnumerable<Product> filteredProducts = pagedProducts.Items;

            if (role != "Admin" && role != "Seller")
            {
                // For non-seller and non-admin roles, filter out unavailable products
                _logger.LogInformation($"Filtering products for role: {role}. Only available products will be returned.");
                filteredProducts = filteredProducts.Where(p => p.IsAvailable);
            }
            return new PagedResult<ProductsUIDto>
            {
                Items = filteredProducts.Select(p => _mapper.Map<ProductsUIDto>(p)).ToList(),
                TotalCount = pagedProducts.TotalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };

        }



        public async Task<IEnumerable<ProductsUIDto>> SearchProductsAsync(string keyword)
        {
            var products = await _unitOfWork.ProductRepo.SearchAsync(keyword);
            if (products == null || !products.Any())
            {
                _logger.LogWarning($"No products found matching the keyword '{keyword}'.");
                return Enumerable.Empty<ProductsUIDto>();
            }
            _logger.LogInformation($"Found {products.Count()} products matching the keyword '{keyword}'.");
             products = products.Where(p=>p.IsAvailable).ToList();
            return _mapper.Map<IEnumerable<ProductsUIDto>>(products);
        }





        public async Task<int> CreateProductAsync(AddProductDto request)
        {

            if (request == null)
                throw new ArgumentNullException(nameof(request));

       
            var product = _mapper.Map<Product>(request);

            product.StockQuantity = request.Variants.Sum(v => v.StockQuantity);
            product.ApprovalStatus = "pending";
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            if (request.MainImageUrl != null && _fileService.IsValidImage(request.MainImageUrl))
            {
                product.MainImageUrl = await _fileService.SaveFileAsync(request.MainImageUrl,"products");
            }
            else
            {
                throw new Exception("Invalid main image URL");
            }

            foreach (var image in request.AdditionalImageUrls)
            {
                if (!_fileService.IsValidImage(image))
                    continue;

                var imageUrl = await _fileService.SaveFileAsync(image, "products");
                product.ProductImages.Add(new ProductImage { ImageUrl = imageUrl });
            }

            var categoryAttributes = await _unitOfWork.ProductAttributeRepo
                .GetAttributesByCategoryIdAsync(request.CategoryId);
            if (request.Variants != null || request.Variants.Any())
            {
                foreach (var attr in request.Attributes)
            {
                var attributeInDb = categoryAttributes
                    .FirstOrDefault(a => a.Name == attr.AttributeName);

                if (attributeInDb == null)
                    throw new Exception($"Attribute '{attr.AttributeName}' not found in category {request.CategoryId}");

                foreach (var value in attr.Values)
                {
                    product.productAttributeValues.Add(new ProductAttributeValue
                    {
                        AttributeId = attributeInDb.AttributeId, 
                        Value = value
                    });
                }
            }

            }

            if (request.Variants != null || request.Variants.Any())
            {
                product.ProductVariants = new List<ProductVariant>(); ;


                    foreach (var variantDto in request.Variants)
                {
                    var variant = _mapper.Map<ProductVariant>(variantDto);
                
                    if(variantDto.VariantImageUrl != null && _fileService.IsValidImage(variantDto.VariantImageUrl))
                    {
                        variant.VariantImageUrl = await _fileService.SaveFileAsync(variantDto.VariantImageUrl, "products");
                    }
                    else
                    {
                        throw new Exception("Invalid variant image URL");
                    }




                    product.ProductVariants.Add(variant);
                }
            }

            // Save everything
            await _unitOfWork.ProductRepo.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return product.ProductId;
        }



        public async Task ActivateProductAsync(int productId)
        {
            await _unitOfWork.ProductRepo.Activate(productId);
            await _unitOfWork.SaveChangesAsync();

        }
        public async Task DeactivateProductAsync(int productId)
        {
            await _unitOfWork.ProductRepo.Deactivate(productId);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task<ProductVariantDto> FindVariantAsync(int productId, FindVariantRequestDto request)
        {
            var product = await _unitOfWork.ProductRepo.GetWithVariantsAndAttributesAsync(productId);

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            var variant = product.ProductVariants
                .FirstOrDefault(v =>
                    request.SelectedAttributes.All(sa =>
                        v.Attributes.Any(a =>
                            a.AttributeName == sa.AttributeName &&
                            a.AttributeValue == sa.AttributeValue))
                );

            if (variant == null)
                throw new KeyNotFoundException("No matching variant found");

            return _mapper.Map<ProductVariantDto>(variant);
        }










        public async Task<AttributeOptionsResponseDto> GetAttributeOptionsAsync(int productId, AttributeOptionsRequestDto request)
        {
            var product = await _unitOfWork.ProductRepo.GetWithVariantsAndAttributesAsync(productId);

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            // Find variants matching current selection
            var matchingVariants = product.ProductVariants
                .Where(v => request.SelectedAttributes
                    .All(sa => v.Attributes
                        .Any(a => a.AttributeName == sa.AttributeName && a.AttributeValue == sa.AttributeValue)))
                .ToList();

            // Find remaining attributes and their valid values
            var allAttributes = product.productAttributeValues
                .Select(pav => pav.ProductAttribute.Name)
                .Distinct();

            var selectedNames = request.SelectedAttributes
                .Select(a => a.AttributeName);

            var remainingAttributes = allAttributes
                .Except(selectedNames);

            var nextOptions = remainingAttributes.Select(attrName => new AttributeOptionDto
            {
                AttributeName = attrName,
                ValidValues = matchingVariants
                    .SelectMany(v => v.Attributes)
                    .Where(a => a.AttributeName == attrName)
                    .Select(a => a.AttributeValue)
                    .Distinct()
                    .ToList()
            }).ToList();

            return new AttributeOptionsResponseDto { NextOptions = nextOptions };
        }


       















    }
}





