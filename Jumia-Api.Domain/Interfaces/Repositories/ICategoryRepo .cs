using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Interfaces.Repositories
{
    public interface ICategoryRepo : IGenericRepo<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesWithSubCategoriesAsync();
        Task<Category> GetCategoryWithSubCategoriesAsync(int id);
        Task<IEnumerable<Category>> GetMainCategoriesAsync();
        Task<List<int>> GetDescendantCategoryIdsAsync(int parentCategoryId);
        Task<bool> HasChildrenAsync(int categoryId);
    }
}
