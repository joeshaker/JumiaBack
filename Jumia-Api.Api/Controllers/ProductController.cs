using Jumia_Api.Application.Dtos.ProductDtos;
using Jumia_Api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jumia_Api.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("get-by-id/{productId:int}")]
        public async Task<IActionResult> GetProductByID(int productId)
        {
           var product = await _productService.GetProductByIdAsync(productId);

            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            return Ok(product);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromBody] AddProductDto product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _productService.CreateProduct(product);
            return Ok();
        }
    }
}
