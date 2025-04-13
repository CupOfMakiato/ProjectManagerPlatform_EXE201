    using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
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

        [HttpGet("result")]
        public async Task<IActionResult> PaymentResult([FromQuery] int orderCode, [FromQuery] string status)
        {
            try
            {
                var paymentStatus = await _paymentService.CheckPaymentStatusAsync(orderCode);
                return Ok(new
                {
                    orderCode,
                    redirectStatus = status, // status nhận từ query string: return/cancel
                    paymentStatus,
                    message = paymentStatus == "PAID" ? "Thanh toán thành công" :
                              paymentStatus == "CANCELLED" ? "Giao dịch bị hủy" :
                              "Giao dịch đang chờ xử lý"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}
