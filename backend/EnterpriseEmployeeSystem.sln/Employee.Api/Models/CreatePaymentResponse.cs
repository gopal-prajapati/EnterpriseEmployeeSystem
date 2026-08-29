namespace EnterpriseEmployeeSystem.Api.Models
{
    public class CreatePaymentResponse
    {
        public int PaymentId { get; set; }

        public int PurchaseId { get; set; }

        public string GatewayOrderId { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = string.Empty;

        public string KeyId { get; set; } = string.Empty;

    }
}
