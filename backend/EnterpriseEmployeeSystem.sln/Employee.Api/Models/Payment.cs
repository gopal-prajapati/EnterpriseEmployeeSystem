using EnterpriseEmployeeSystem.Api.Enum;

namespace EnterpriseEmployeeSystem.Api.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int PurchaseId { get; set; }

        public string Gateway { get; set; } = string.Empty;

        public string? GatewayOrderId { get; set; }

        public string? GatewayPaymentId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "INR";

        public PaymentStatus Status { get; set; } = PaymentStatus.Created;

        public int AttemptNumber { get; set; }

        public string? GatewayErrorCode { get; set; }

        public string? GatewayErrorDescription { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAtUtc { get; set; }

        public Purchase Purchase { get; set; } = null!;
    }
}
