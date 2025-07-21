using Jumia_Api.Application.Dtos.WishlistDtos;
using Jumia_Api.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jumia_Api.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {
            int customerId = GetCustomerId();
            var wishlist = await _wishlistService.GetWishlistAsync(customerId);
            return Ok(wishlist);
        }

        [HttpPost("items/{productId}")]
        public async Task<IActionResult> AddItem( int productId)
        {
            int customerId = GetCustomerId();
            await _wishlistService.AddItemAsync(customerId, productId);
            return Ok();
        }

        [HttpDelete("items/{id}")]
        public async Task<IActionResult> RemoveItem(int id)
        {
            int customerId = GetCustomerId();
            await _wishlistService.RemoveItemAsync(customerId, id);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> ClearWishlist()
        {
            int customerId = GetCustomerId();
            await _wishlistService.ClearWishlistAsync(customerId);
            return Ok();
        }

        private int GetCustomerId()
        {
            return int.Parse(User.FindFirst("userTypeId").Value);
        }
    }

}