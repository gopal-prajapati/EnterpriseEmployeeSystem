using EnterpriseEmployeeSystem.Api.Models;

namespace EnterpriseEmployeeSystem.Api.Services.Products
{
    public interface IProductService
    {
        Task<Product?> GetActiveByItemCodeAsync(string itemCode);

    }
}
