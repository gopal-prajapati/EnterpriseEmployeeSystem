namespace EnterpriseEmployeeSystem.Api.Gateways.Payments
{
    public interface IPaymentGateway
    {
        Task<PaymentGatewayOrderResponse> CreateOrderAsync(
    PaymentGatewayOrderRequest request);

    }
}
