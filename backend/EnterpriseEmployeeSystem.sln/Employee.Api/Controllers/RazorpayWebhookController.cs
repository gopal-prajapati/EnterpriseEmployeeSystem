using EnterpriseEmployeeSystem.Api.Gateways.Payments;
using EnterpriseEmployeeSystem.Api.Models;
using EnterpriseEmployeeSystem.Api.Repositories.Payments;
using EnterpriseEmployeeSystem.Api.Repositories.Webhook;
using EnterpriseEmployeeSystem.Api.Services.Payments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace EnterpriseEmployeeSystem.Api.Controllers
{
    [ApiController]
    [Route("api/webhooks/razorpay")]
    public class RazorpayWebhookController : ControllerBase
    {
        private readonly RazorpayOptions _razorpayOptions;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentService _paymentService;
        private readonly IWebhookEventRepository _webhookEventRepository;
        public RazorpayWebhookController(
            IOptions<RazorpayOptions> razorpayOptions,
            IPaymentRepository paymentRepository,
            IPaymentService paymentService,
            IWebhookEventRepository webhookEventRepository)
        {
            _razorpayOptions = razorpayOptions.Value;
            _paymentRepository = paymentRepository;
            _paymentService = paymentService;
            _webhookEventRepository = webhookEventRepository;
        }

        [HttpPost]
        public async Task<IActionResult> HandleWebhook()
        {
            Request.EnableBuffering();

            using var reader = new StreamReader(
                Request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);

            var rawBody = await reader.ReadToEndAsync();

            Request.Body.Position = 0;

            var signature =
                Request.Headers["X-Razorpay-Signature"]
                    .FirstOrDefault();

            var eventId =
                Request.Headers["X-Razorpay-Event-Id"]
                    .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(signature))
            {
                return Unauthorized();
            }

            var isValid =
                VerifyWebhookSignature(
                    rawBody,
                    signature,
                    _razorpayOptions.WebhookSecret);

            if (!isValid)
            {
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(eventId))
            {
                return BadRequest();
            }

            var alreadyProcessed =
    await _webhookEventRepository.ExistsAsync(eventId);

            if (alreadyProcessed)
            {
                Console.WriteLine(
                    $"Webhook event already processed: {eventId}");

                return Ok();
            }

            using var document =
       JsonDocument.Parse(rawBody);

            var root =
                document.RootElement;

            var eventName =
                root.GetProperty("event")
                    .GetString();

            Console.WriteLine(
                $"Webhook Event: {eventName}");

            if (eventName != "payment.captured")
            {
                Console.WriteLine(
                    $"Ignoring unsupported event: {eventName}");

                return Ok();
            }

            var paymentEntity =
                root
                    .GetProperty("payload")
                    .GetProperty("payment")
                    .GetProperty("entity");

            var razorpayPaymentId =
                paymentEntity.GetProperty("id")
                    .GetString();

            var razorpayOrderId =
                paymentEntity.GetProperty("order_id")
                    .GetString();

            var amountInPaise =
                paymentEntity.GetProperty("amount")
                    .GetInt64();

            var currency =
                paymentEntity.GetProperty("currency")
                    .GetString();

            var paymentStatus =
                paymentEntity.GetProperty("status")
                    .GetString();

            if (string.IsNullOrWhiteSpace(razorpayOrderId) ||
     string.IsNullOrWhiteSpace(razorpayPaymentId))
            {
                return BadRequest();
            }

            var payment =
    await _paymentRepository
        .GetByGatewayOrderIdAsync(razorpayOrderId);

            if (payment == null)
            {
                Console.WriteLine(
                    $"No local Payment found for Razorpay order {razorpayOrderId}");

                return Ok();
            }

            var webhookAmount =
    amountInPaise / 100m;

            if (payment.Amount != webhookAmount ||
    !string.Equals(
        payment.Currency,
        currency,
        StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine(
                    $"Webhook amount/currency mismatch for Payment {payment.Id}");

                return BadRequest();
            }

            await _paymentService.CompletePaymentAsync(
    payment,
    razorpayPaymentId);

            await _webhookEventRepository.AddAsync(
    new WebhookEvent
    {
        EventId = eventId,
        EventType = eventName ?? string.Empty,
        ProcessedAtUtc = DateTime.UtcNow
    });


            return Ok();
        }

        private static bool VerifyWebhookSignature(
            string rawBody,
            string receivedSignature,
            string webhookSecret)
        {
            using var hmac =
                new HMACSHA256(
                    Encoding.UTF8.GetBytes(webhookSecret));

            var hash =
                hmac.ComputeHash(
                    Encoding.UTF8.GetBytes(rawBody));

            var generatedSignature =
                Convert.ToHexString(hash)
                    .ToLowerInvariant();

            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(generatedSignature),
                Encoding.UTF8.GetBytes(receivedSignature));
        }
    }
}