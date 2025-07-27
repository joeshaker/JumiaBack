using Jumia_Api.Application.Interfaces;
using Jumia_Api.Application.Dtos.PaymentDtos;
using Microsoft.AspNetCore.Mvc;
using Jumia_Api.Application.Dtos.OrderDtos;

namespace Jumia_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("initiate")]
        public async Task<IActionResult> InitiatePayment([FromBody] CreateOrderDTO request)
        {
            var response = await _paymentService.InitiatePaymentAsync(request);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> HandleCallback([FromQuery] Dictionary<string, string> queryParams)
        {
            // Optional: Log or debug query parameters
            //foreach (var param in queryParams)
            //{
            //    Console.WriteLine($"{param.Key} = {param.Value}");
            //}

            // Example: pass the dictionary as a stringified payload if needed
            var success = await _paymentService.ValidatePaymentCallback(System.Text.Json.JsonSerializer.Serialize(queryParams));

            if (!success)
                return BadRequest("Invalid callback");

            return Ok("Callback processed");
        }

    }
}
