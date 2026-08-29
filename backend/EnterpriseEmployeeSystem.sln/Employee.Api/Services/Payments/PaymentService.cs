using EnterpriseEmployeeSystem.Api.DTOs;
using EnterpriseEmployeeSystem.Api.Enum;
using EnterpriseEmployeeSystem.Api.Gateways.Payments;
using EnterpriseEmployeeSystem.Api.Models;
using EnterpriseEmployeeSystem.Api.Repositories.Payments;
using EnterpriseEmployeeSystem.Api.Repositories.Purchases;

namespace EnterpriseEmployeeSystem.Api.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentGateway _paymentGateway;


        public PaymentService(
            IPurchaseRepository purchaseRepository,
            IPaymentRepository paymentRepository,
            IPaymentGateway paymentGateway)
        {
            _purchaseRepository = purchaseRepository;
            _paymentRepository = paymentRepository;
            _paymentGateway = paymentGateway;

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
    }
}