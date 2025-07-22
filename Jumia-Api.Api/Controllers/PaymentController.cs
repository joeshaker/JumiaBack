using Jumia_Api.Application.Interfaces;
using Jumia_Api.Application.Dtos.PaymentDtos;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> InitiatePayment([FromBody] PaymentRequetsDto request)
        {
            var response = await _paymentService.InitiatePaymentAsync(request);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("callback")]
        public async Task<IActionResult> HandleCallback([FromForm] string payload)
        {
            var success = await _paymentService.ValidatePaymentCallback(payload.ToString());

            if (!success)
                return BadRequest("Invalid callback");

            return Ok("Callback processed");
        }
    }
}
