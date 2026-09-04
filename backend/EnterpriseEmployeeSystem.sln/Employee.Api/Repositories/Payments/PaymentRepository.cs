using EnterpriseEmployeeSystem.Api.Data;
using EnterpriseEmployeeSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseEmployeeSystem.Api.Repositories.Payments
{
    public class PaymentRepository: IPaymentRepository
    {
        private readonly AppDbContext _dbContext;

        public PaymentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetAttemptCountAsync(int purchaseId)
        {
            return await _dbContext.Payments
                .CountAsync(x => x.PurchaseId == purchaseId);
        }

        public async Task<Payment> AddAsync(Payment payment)
        {
            _dbContext.Payments.Add(payment);

            await _dbContext.SaveChangesAsync();

            return payment;
        }

        public async Task<Payment> UpdateAsync(Payment payment)
        {
            _dbContext.Payments.Update(payment);

            await _dbContext.SaveChangesAsync();

            return payment;
        }

        public async Task<Payment?> GetByIdAsync(int paymentId)
        {
            return await _dbContext.Payments
                .FirstOrDefaultAsync(x => x.Id == paymentId);
        }

        public async Task<Payment?> GetByGatewayOrderIdAsync(
    string gatewayOrderId)
        {
            return await _dbContext.Payments
                .FirstOrDefaultAsync(x =>
                    x.GatewayOrderId == gatewayOrderId);
        }

    }
}
