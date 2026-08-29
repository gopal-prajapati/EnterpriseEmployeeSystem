namespace EnterpriseEmployeeSystem.Api.Gateways.Payments
{
    public class SandboxPaymentGateway : IPaymentGateway
    {
        public Task<PaymentGatewayOrderResponse> CreateOrderAsync(
    PaymentGatewayOrderRequest request)
        {
            var response = new PaymentGatewayOrderResponse
            {
                GatewayOrderId = $"sandbox_order_{Guid.NewGuid():N}",
                Amount = request.Amount,
                Currency = request.Currency,
                Status = "created"
            };

            return Task.FromResult(response);
        }

    }
}
