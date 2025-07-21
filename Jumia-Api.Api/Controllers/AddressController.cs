using Jumia_Api.Application.Dtos.AddressDtos;
using Jumia_Api.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jumia_Api.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }



        // GET: api/Address
        [HttpGet]
        public async Task<IActionResult> GetAllAddressesOfAllUsers()
        {
            var addresses = await _addressService.GetAllAddressesOfAllUsers();
            return Ok(addresses);
        }



        // GET: api/Address/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllAddressesByUserId(string userId)
        {
            var addresses = await _addressService.GetAllAddressesByUserId(userId);
            return Ok(addresses);
        }

       

        // GET: api/Address/{addressId}
        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetAddressById(int addressId)
        {
            var address = await _addressService.GetAddressById(addressId);
            if (address == null)
            {
                return NotFound();
            }
            return Ok(address);
        }



        // POST: api/Address
        [HttpPost]
        public async Task<IActionResult> AddNewAddress([FromBody] CreateAddressDto addressDto)
        {
            if (addressDto == null)
            {
                return BadRequest("Address data is required.");
            }
            var newAddress = await _addressService.AddNewAddress(addressDto);
            return CreatedAtAction(nameof(GetAddressById), new { addressId = newAddress.AddressId }, newAddress);
        }



        [HttpPut("{addressId}")]
        public async Task<IActionResult> UpdateAddress(int addressId, [FromBody] UpdateAddressDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Address data is required.");
            }
            var result = await _addressService.UpdateAddress(addressId, dto);
            if (!result)
            {
                return NotFound($"Address with ID {addressId} not found.");
            }
            return NoContent();
        }




        [HttpDelete("{addressId}")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            var result = await _addressService.DeleteAddress(addressId);
            if (!result)
            {
                return NotFound($"Address with ID {addressId} not found.");
            }
            return NoContent();
        }



    }
}
