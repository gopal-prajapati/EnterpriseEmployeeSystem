using EnterpriseEmployeeSystem.Api.Data;
using EnterpriseEmployeeSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseEmployeeSystem.Api.Repositories.Products
{
    public class ProductRepository: IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product?> GetActiveByItemCodeAsync(string itemCode)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.ItemCode == itemCode &&
                    x.IsActive);
        }

    }
}
