using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Interfaces.Repositories
{
    public interface IProductAttributeRepo : IGenericRepo<ProductAttribute>
    {
        public Task<List<ProductAttribute>> GetAttributesByCategoryIdAsync(int categoryId);
        public Task<List<ProductAttribute>> GetAttributesForCategoriesAsync(List<int> categoryIds);
    }
}
