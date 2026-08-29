namespace EnterpriseEmployeeSystem.Api.Gateways.Payments
{
    public class PaymentGatewayOrderResponse
    {
        public string GatewayOrderId { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

    }
}
