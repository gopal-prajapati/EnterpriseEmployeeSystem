using EnterpriseEmployeeSystem.Api.Models;

namespace EnterpriseEmployeeSystem.Api.Repositories.Products
{
    public interface IProductRepository
    {
        Task<Product?> GetActiveByItemCodeAsync(string itemCode);

    }
}
