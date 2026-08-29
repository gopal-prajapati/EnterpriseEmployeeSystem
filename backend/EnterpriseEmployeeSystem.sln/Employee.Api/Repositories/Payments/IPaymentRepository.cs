using EnterpriseEmployeeSystem.Api.Models;

namespace EnterpriseEmployeeSystem.Api.Repositories.Payments
{
    public interface IPaymentRepository
    {
        Task<int> GetAttemptCountAsync(int purchaseId);

        Task<Payment> AddAsync(Payment payment);

        Task<Payment> UpdateAsync(Payment payment);

    }
}
