using Jumia_Api.Application.Dtos.CategoryDtos;
using Jumia_Api.Application.Dtos.ProductDtos;
using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryDto createDto);
        Task DeleteCategoryAsync(int id);
        Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync(bool includeSubcategories = false);
        Task<CategoryResponseDto> GetCategoryByIdAsync(int id, bool includeSubcategories = false);
        Task<IEnumerable<CategoryResponseDto>> GetMainCategoriesAsync();
        Task<CategoryResponseDto> UpdateCategoryAsync(int id, UpdateCategoryDto updateDto);
        Task<bool> CategoryNameExistsAsync(string name);
        Task<IEnumerable<int>> GetCategoryAndDescendantIdsAsync(int categoryId);
        Task MoveCategoryToNewParentAsync(int categoryId, MoveCategoryDto moveDto);
        Task<IEnumerable<ProductAttribute>> GetCategoriesAttributes(int parentId);
    }
}
