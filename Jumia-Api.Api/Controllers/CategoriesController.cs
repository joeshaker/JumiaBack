using Jumia_Api.Application.Dtos.CategoryDtos;
using Jumia_Api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jumia_Api.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAll([FromQuery] bool includeSubcategories = false)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(includeSubcategories);
            return categories.Any() ? Ok(categories) : NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryResponseDto>> GetById([FromRoute] int id,[FromQuery] bool includeSubcategories = false)
        {
            if (id <= 0)
                return BadRequest("Category ID must be positive");

            var category = await _categoryService.GetCategoryByIdAsync(id, includeSubcategories);

            return category != null
                ? Ok(category)
                : NotFound($"Category with ID {id} not found");
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> Create([FromBody] CreateCategoryDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _categoryService.CategoryNameExistsAsync(createDto.Name))
                return Conflict($"Category '{createDto.Name}' already exists");

            var createdCategory = await _categoryService.CreateCategoryAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody] UpdateCategoryDto updateDto)
        {
            if (id <= 0) return BadRequest("Invalid category ID");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _categoryService.UpdateCategoryAsync(id, updateDto);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest("Category ID must be positive");

            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
        [HttpGet("{id:int}/descendants")]
        public async Task<ActionResult<IEnumerable<int>>> GetDescendants([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Invalid category ID");

            var descendants = await _categoryService.GetCategoryAndDescendantIdsAsync(id);
            return descendants.Any() ? Ok(descendants) : NotFound();
        }

        [HttpGet("main")]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetMainCategories()
        {
            var categories = await _categoryService.GetMainCategoriesAsync();
            return categories.Any() ? Ok(categories) : NoContent();
        }

        [HttpGet("{parentId:int}/attributes")]
        public async Task<IActionResult> GetAttributes(int parentId)
        {
            if (parentId <= 0)
                return BadRequest("Parent category ID must be positive");
            var attributes = await _categoryService.GetCategoriesAttributes(parentId);
            return attributes.Any() ? Ok(attributes) : NotFound($"No attributes found for category with ID {parentId}");
        }
    }
}
