using EnterpriseEmployeeSystem.Api.Data;
using EnterpriseEmployeeSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseEmployeeSystem.Api.Repositories.Purchases
{
    public class PurchaseRepository: IPurchaseRepository
    {
        private readonly AppDbContext _dbContext;

        public PurchaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Purchase> AddAsync(Purchase purchase)
        {
            _dbContext.Purchases.Add(purchase);

            await _dbContext.SaveChangesAsync();

            return purchase;
        }

        public async Task<Purchase?> GetByIdAsync(int purchaseId)
        {
            return await _dbContext.Purchases
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == purchaseId);
        }

    }
}
