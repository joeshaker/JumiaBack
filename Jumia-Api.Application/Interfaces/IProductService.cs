using Jumia_Api.Application.Dtos.ProductDtos;
using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetAllProductsAsync();
        public Task<IEnumerable<Product>> GetAvailableProductsAsync();
        public Task<ProductDto> GetProductByIdAsync(int productId);
        public Task UpdateProduct(UpdateProductDto product);
        public Task CreateProduct(AddProductDto product);
        public Task DeleteProduct(int productId);
        public Task<IEnumerable<Product>> GetProductsByCategoriesAsync(ProductFilterRequestDto productFilterRequestDto);

    }
}
