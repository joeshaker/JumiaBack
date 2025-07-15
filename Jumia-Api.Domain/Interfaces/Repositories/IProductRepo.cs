using Jumia_Api.Domain.Models;

namespace Jumia_Api.Domain.Interfaces.Repositories
{
    public interface IProductRepo:IGenericRepo<Product>
    {
        public Task<IEnumerable<Product>> GetAvailableProductsAsync();
        public Task<List<Product>> GetProductsByCategoryIdsAsync(List<int> categoryIds,
                                                                Dictionary<string, string> attributeFilters = null,
                                                                decimal? minPrice = null,
                                                                decimal? maxPrice = null);

        
    }
}
