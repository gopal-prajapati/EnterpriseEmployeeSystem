using EnterpriseEmployeeSystem.Api.Models;

namespace EnterpriseEmployeeSystem.Api.Repositories.Purchases
{
    public interface IPurchaseRepository
    {
        Task<Purchase> AddAsync(Purchase purchase);

        Task<Purchase?> GetByIdAsync(int purchaseId);

        Task<Purchase> UpdateAsync(Purchase purchase);

    }
}
