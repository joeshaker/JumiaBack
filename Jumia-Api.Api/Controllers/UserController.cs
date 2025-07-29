using Jumia_Api.Application.Dtos.UserDtos;
using Jumia_Api.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OllamaSharp.Models.Chat;
using System.Security.Claims;

namespace Jumia_Api.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("profile")]
        
        public async Task<ActionResult<UserProfileDto>> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await _userService.GetUserProfileAsync(userId));
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto updateDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _userService.UpdateUserProfileAsync(userId, updateDto);
            return NoContent();
        }

        [HttpGet("customers")]
        public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetAllCustomers()
        {
            try
            {
                var customers = await _userService.GetAllCustomersAsync();
                return Ok(customers);
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("sellers")]
        public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetAllSellers()
        {
            var sellers = await _userService.GetAllSellersAsync();
            return Ok(sellers);
        }

        [HttpGet("admin")]
        public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetAdmin()
        {
            var admin = await _userService.GetAllAdminAsync();
            return Ok(admin);
        }

        [HttpPost("toggle-block-status/{customerId}")]
        public async Task<IActionResult> ToggleBlockStatus(int customerId)
        {
            var isBlocked = await _userService.ToogleBlockStatusAsync(customerId);

            if (isBlocked)
            {
                return Ok(new { Message="Customer has been blocked.", isBlocked });
            }

            return Ok(new { Message= "Customer has been unblocked.", isBlocked });
        }

    }
}
