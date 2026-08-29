using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EnterpriseEmployeeSystem.Api.Gateways.Payments
{
    public class RazorpayPaymentGateway: IPaymentGateway
    {
        private readonly HttpClient _httpClient;
        private readonly RazorpayOptions _options;

        public RazorpayPaymentGateway(
            HttpClient httpClient,
            IOptions<RazorpayOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<PaymentGatewayOrderResponse> CreateOrderAsync(
            PaymentGatewayOrderRequest request)
        {
            var amountInPaise =
                checked((long)(request.Amount * 100m));

            var razorpayRequest = new
            {
                amount = amountInPaise,
                currency = request.Currency,
                receipt = request.Receipt
            };

            var json = JsonSerializer.Serialize(razorpayRequest);

            using var httpRequest =
                new HttpRequestMessage(
                    HttpMethod.Post,
                    "v1/orders");

            httpRequest.Content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var credentials =
                Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        $"{_options.KeyId}:{_options.KeySecret}"));

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Basic",
                    credentials);

            using var response =
                await _httpClient.SendAsync(httpRequest);

            var responseBody =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Razorpay order creation failed. Status: {(int)response.StatusCode}");
            }

            using var document =
                JsonDocument.Parse(responseBody);

            var root = document.RootElement;

            var gatewayOrderId =
                root.GetProperty("id").GetString();

            var gatewayAmount =
                root.GetProperty("amount").GetInt64();

            var currency =
                root.GetProperty("currency").GetString();

            var status =
                root.GetProperty("status").GetString();

            if (string.IsNullOrWhiteSpace(gatewayOrderId))
            {
                throw new InvalidOperationException(
                    "Razorpay returned an invalid order id.");
            }

            return new PaymentGatewayOrderResponse
            {
                GatewayOrderId = gatewayOrderId,

                Amount = gatewayAmount / 100m,

                Currency = currency ?? request.Currency,

                Status = status ?? string.Empty
            };
        }

    }
}
