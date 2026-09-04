using EnterpriseEmployeeSystem.Api.DTOs;
using EnterpriseEmployeeSystem.Api.Gateways.Payments;
using EnterpriseEmployeeSystem.Api.Models;
using EnterpriseEmployeeSystem.Api.Services.Payments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EnterpriseEmployeeSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly RazorpayOptions _razorpayOptions;


        public PaymentsController(IPaymentService paymentService, IOptions<RazorpayOptions> razorpayOptions)
        {
            _paymentService = paymentService;
            _razorpayOptions = razorpayOptions.Value;

        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment(
            CreatePaymentRequest request)
        {
            try
            {
                var payment =
                    await _paymentService.CreatePaymentAsync(request);

                var response = new CreatePaymentResponse
                {
                    PaymentId = payment.Id,
                    PurchaseId = payment.PurchaseId,
                    GatewayOrderId = payment.GatewayOrderId ?? string.Empty,
                    Amount = payment.Amount,
                    Currency = payment.Currency,
                    KeyId = _razorpayOptions.KeyId
                };

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyPayment(
    VerifyPaymentRequest request)
        {
            try
            {
                await _paymentService
                    .VerifyPaymentAsync(request);

                return Ok(new
                {
                    message = "Payment verified successfully."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
