using EnterpriseEmployeeSystem.Api.Data;
using EnterpriseEmployeeSystem.Api.DTOs;
using EnterpriseEmployeeSystem.Api.Enum;
using EnterpriseEmployeeSystem.Api.Gateways.Payments;
using EnterpriseEmployeeSystem.Api.Models;
using EnterpriseEmployeeSystem.Api.Repositories.Payments;
using EnterpriseEmployeeSystem.Api.Repositories.Purchases;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace EnterpriseEmployeeSystem.Api.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentGateway _paymentGateway;
        private readonly RazorpayOptions _razorpayOptions;
        private readonly AppDbContext _dbContext;


        public PaymentService(
            IPurchaseRepository purchaseRepository,
            IPaymentRepository paymentRepository,
            IPaymentGateway paymentGateway,
            IOptions<RazorpayOptions> razorpayOptions,
            AppDbContext dbContext)
        {
            _purchaseRepository = purchaseRepository;
            _paymentRepository = paymentRepository;
            _paymentGateway = paymentGateway;
            _razorpayOptions = razorpayOptions.Value;
            _dbContext = dbContext;

        }

        public async Task<Payment> CreatePaymentAsync(
           CreatePaymentRequest request)
        {
            var purchase =
                await _purchaseRepository.GetByIdAsync(request.PurchaseId);

            if (purchase == null)
            {
                throw new InvalidOperationException("Purchase not found.");
            }

            if (purchase.Status != PurchaseStatus.Pending)
            {
                throw new InvalidOperationException(
                    "Payment can only be created for a pending purchase.");
            }

            var existingAttempts =
                await _paymentRepository
                    .GetAttemptCountAsync(purchase.Id);

            var payment = new Payment
            {
                PurchaseId = purchase.Id,
                Gateway = "Razorpay",
                Amount = purchase.Amount,
                Currency = purchase.Currency,
                Status = PaymentStatus.Created,
                AttemptNumber = existingAttempts + 1,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            payment = await _paymentRepository.AddAsync(payment);

            var gatewayRequest = new PaymentGatewayOrderRequest
            {
                PurchaseId = purchase.Id,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Receipt = $"purchase_{purchase.Id}_attempt_{payment.AttemptNumber}"
            };

            var gatewayResponse =
                await _paymentGateway.CreateOrderAsync(gatewayRequest);

            payment.GatewayOrderId =
                gatewayResponse.GatewayOrderId;

            payment.Status = PaymentStatus.Pending;

            payment.UpdatedAtUtc = DateTime.UtcNow;

            return await _paymentRepository.UpdateAsync(payment);
        }

        public async Task VerifyPaymentAsync(
    VerifyPaymentRequest request)
        {
                        
                var payment =
               await _paymentRepository.GetByIdAsync(request.PaymentId);

                if (payment == null)
                {
                    throw new InvalidOperationException(
                        "Payment not found.");
                }

                if (payment.Status == PaymentStatus.Succeeded)
                {
                    if (payment.GatewayOrderId == request.RazorpayOrderId &&
                        payment.GatewayPaymentId == request.RazorpayPaymentId)
                    {
                        return;
                    }

                    throw new InvalidOperationException(
                        "Payment has already been completed with different payment details.");
                }

                if (payment.GatewayOrderId != request.RazorpayOrderId)
                {
                    throw new InvalidOperationException(
                        "Payment order does not match.");
                }

                var payload =
                    $"{payment.GatewayOrderId}|{request.RazorpayPaymentId}";

                using var hmac =
                    new HMACSHA256(
                        Encoding.UTF8.GetBytes(
                            _razorpayOptions.KeySecret));

                var hash =
                    hmac.ComputeHash(
                        Encoding.UTF8.GetBytes(payload));

                var generatedSignature =
                    Convert.ToHexString(hash)
                        .ToLowerInvariant();

                var signaturesMatch =
                    CryptographicOperations.FixedTimeEquals(
                        Encoding.UTF8.GetBytes(generatedSignature),
                        Encoding.UTF8.GetBytes(request.RazorpaySignature));

                if (!signaturesMatch)
                {
                    throw new InvalidOperationException(
                        "Invalid payment signature.");
                }

            //            payment.GatewayPaymentId =
            //                request.RazorpayPaymentId;

            //            payment.Status =
            //                PaymentStatus.Succeeded;

            //            payment.CompletedAtUtc =
            //                DateTime.UtcNow;

            //            payment.UpdatedAtUtc =
            //                DateTime.UtcNow;

            //        await using var transaction =
            //await _dbContext.Database.BeginTransactionAsync();

            //        try
            //        {

            //            await _paymentRepository.UpdateAsync(payment);

            //            var purchase =
            //                await _purchaseRepository
            //                    .GetByIdAsync(payment.PurchaseId);

            //            if (purchase == null)
            //            {
            //                throw new InvalidOperationException(
            //                    "Purchase not found.");
            //            }

            //            purchase.Status =
            //                PurchaseStatus.Paid;

            //            purchase.UpdatedAtUtc =
            //                DateTime.UtcNow;

            //            await _purchaseRepository.UpdateAsync(purchase);
            //            await transaction.CommitAsync();

            //        }
            //        catch (Exception ex)
            //        {
            //            await transaction.RollbackAsync();
            //            throw;
            //        }

           await CompletePaymentAsync(payment, request.RazorpayPaymentId);
           
        }

        public async Task CompletePaymentAsync(
    Payment payment,
    string razorpayPaymentId)
        {
            if (payment.Status == PaymentStatus.Succeeded)
            {
                if (payment.GatewayPaymentId == razorpayPaymentId)
                {
                    return;
                }

                throw new InvalidOperationException(
                    "Payment has already been completed with different payment details.");
            }

            var purchase =
                await _purchaseRepository
                    .GetByIdAsync(payment.PurchaseId);

            if (purchase == null)
            {
                throw new InvalidOperationException(
                    "Purchase not found.");
            }

            await using var transaction =
                await _dbContext.Database.BeginTransactionAsync();

            try
            {
                payment.GatewayPaymentId =
                    razorpayPaymentId;

                payment.Status =
                    PaymentStatus.Succeeded;

                payment.CompletedAtUtc =
                    DateTime.UtcNow;

                payment.UpdatedAtUtc =
                    DateTime.UtcNow;

                await _paymentRepository.UpdateAsync(payment);

                purchase.Status =
                    PurchaseStatus.Paid;

                purchase.UpdatedAtUtc =
                    DateTime.UtcNow;

                await _purchaseRepository.UpdateAsync(purchase);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}