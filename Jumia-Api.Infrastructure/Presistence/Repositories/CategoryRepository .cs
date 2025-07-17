using Jumia_Api.Domain.Interfaces.Repositories;
using Jumia_Api.Domain.Models;
using Jumia_Api.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;


namespace Jumia_Api.Infrastructure.Presistence.Repositories
{
    public class CategoryRepository : GenericRepo<Category>, ICategoryRepo
    {
        public CategoryRepository(JumiaDbContext context) : base(context)
        {
        }


        public override async Task AddAsync(Category entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                throw new System.ArgumentException("Category name cannot be empty");
            }

            await base.AddAsync(entity);
        }

        public override async Task Delete(int id)
        {
            bool hasChildren = await _dbSet
                .AnyAsync(c => c.ParentCategoryId == id);

            if (hasChildren)
            {
                throw new System.Exception("Cannot delete category with subcategories");
            }

            await base.Delete(id);
        }

        public override async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<Category?> GetByIdAsync(int id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public override void Update(Category entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                throw new System.ArgumentException("Category name cannot be empty");
            }

            base.Update(entity);
        }

        public async Task<List<int>> GetDescendantCategoryIdsAsync(int parentCategoryId)
        {
            var childIds = await _dbSet
                .Where(c => c.ParentCategoryId == parentCategoryId)
                .Select(c => c.Id)
                .ToListAsync();

            var allDescendants = new List<int>(childIds);

            foreach (var childId in childIds)
            {
                var grandchildren = await GetDescendantCategoryIdsAsync(childId);
                allDescendants.AddRange(grandchildren);
            }

            return allDescendants;
        }

        public async Task<List<int>> GetChildCategoryIdsAsync(int parentCategoryId)
        {
            return await _dbSet
                .Where(c => c.ParentCategoryId == parentCategoryId)
                .Select(c => c.Id)
                .ToListAsync();
        }

        public async Task<bool> HasChildrenAsync(int categoryId)
        {
            return await _dbSet
                .AnyAsync(c => c.ParentCategoryId == categoryId);
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithSubCategoriesAsync()
        {
            var allCategories = await _dbSet
                .AsNoTracking()
                .ToListAsync();

            return BuildCategoryHierarchy(allCategories);
        }

        public async Task<Category> GetCategoryWithSubCategoriesAsync(int id)
        {
            var allCategories = await _dbSet
                .AsNoTracking()
                .ToListAsync();

            var category = allCategories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                category.SubCategories = allCategories
                    .Where(c => c.ParentCategoryId == id)
                    .ToList();
            }

            return category;
        }

        public async Task<IEnumerable<Category>> GetMainCategoriesAsync()
        {
            var allCategories = await _dbSet
                .AsNoTracking()
                .ToListAsync();

            var mainCategories = allCategories
                .Where(c => c.ParentCategoryId == null)
                .ToList();

            
            return mainCategories;
        }


        private IEnumerable<Category> BuildCategoryHierarchy(List<Category> allCategories)
        {
            var rootCategories = allCategories
                .Where(c => c.ParentCategoryId == null)
                .ToList();

            foreach (var category in rootCategories)
            {
                BuildSubCategoryTree(category, allCategories);
            }

            return rootCategories;
        }

        private void BuildSubCategoryTree(Category parent, List<Category> allCategories)
        {
            var children = allCategories
                .Where(c => c.ParentCategoryId == parent.Id)
                .ToList();

            parent.SubCategories = children;

            foreach (var child in children)
            {
                BuildSubCategoryTree(child, allCategories); // Recursively build tree for each child
            }
        }
    }
}




