namespace EnterpriseEmployeeSystem.Api.Gateways.Payments
{
    public class PaymentGatewayOrderRequest
    {
        public int PurchaseId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = string.Empty;

        public string Receipt { get; set; } = string.Empty;

    }
}
