using Jumia_Api.Domain.Interfaces.Repositories;
using Jumia_Api.Domain.Models;
using Jumia_Api.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Jumia_Api.Infrastructure.Presistence.Repositories
{
    public class ProductAttributeRepo : GenericRepo<ProductAttribute>, IProductAttributeRepo
    {
        public ProductAttributeRepo(JumiaDbContext context) : base(context)
        {
        }

        public async Task<List<ProductAttribute>> GetAttributesForCategoriesAsync(List<int> categoryIds)
        => await  _dbSet
            .Where(attr => categoryIds.Contains(attr.Category.Id))
            .AsNoTracking()
            .ToListAsync();
    }
}
