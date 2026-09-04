using EnterpriseEmployeeSystem.Api.Models;

namespace EnterpriseEmployeeSystem.Api.Repositories.Payments
{
    public interface IPaymentRepository
    {
        Task<int> GetAttemptCountAsync(int purchaseId);

        Task<Payment> AddAsync(Payment payment);

        Task<Payment> UpdateAsync(Payment payment);

        Task<Payment?> GetByIdAsync(int paymentId);

        Task<Payment?> GetByGatewayOrderIdAsync(string gatewayOrderId);

    }
}
