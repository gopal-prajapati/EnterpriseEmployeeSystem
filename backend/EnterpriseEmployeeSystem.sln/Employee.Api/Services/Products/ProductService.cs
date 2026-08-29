using EnterpriseEmployeeSystem.Api.Models;
using EnterpriseEmployeeSystem.Api.Repositories.Products;

namespace EnterpriseEmployeeSystem.Api.Services.Products
{
    public class ProductService: IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product?> GetActiveByItemCodeAsync(string itemCode)
        {
            return await _productRepository
                .GetActiveByItemCodeAsync(itemCode);
        }
    }
}
