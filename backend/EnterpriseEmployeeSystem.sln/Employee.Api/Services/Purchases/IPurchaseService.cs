using EnterpriseEmployeeSystem.Api.DTOs;
using EnterpriseEmployeeSystem.Api.Models;

namespace EnterpriseEmployeeSystem.Api.Services.Purchases
{
    public interface IPurchaseService
    {
        Task<Purchase> CreatePurchaseAsync(CreatePurchaseRequest request);

    }
}
