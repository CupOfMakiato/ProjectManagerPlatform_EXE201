    using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Interfaces;
using Server.Contracts.DTO.Payment;
using System;
using System.Threading.Tasks;

namespace Server.API.Controllers
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

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest createPaymentRequest)
            {
            try
            {
                var paymentUrl = await _paymentService.CreatePaymentLinkAsync(createPaymentRequest);
                return Ok(new { paymentUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
