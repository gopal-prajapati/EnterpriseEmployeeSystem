using EnterpriseEmployeeSystem.Api.Data;
using EnterpriseEmployeeSystem.Api.DTOs;
using EnterpriseEmployeeSystem.Api.Enum;
using EnterpriseEmployeeSystem.Api.Models;
using EnterpriseEmployeeSystem.Api.Repositories;
using EnterpriseEmployeeSystem.Api.Repositories.Products;
using EnterpriseEmployeeSystem.Api.Repositories.Purchases;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseEmployeeSystem.Api.Services.Purchases
{
    public class PurchaseService: IPurchaseService
    {


        private readonly IEmployeeRepository _employeeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPurchaseRepository _purchaseRepository;

        public PurchaseService(
            IEmployeeRepository employeeRepository,
            IProductRepository productRepository,
            IPurchaseRepository purchaseRepository)
        {
            _employeeRepository = employeeRepository;
            _productRepository = productRepository;
            _purchaseRepository = purchaseRepository;
        }

        public async Task<Purchase> CreatePurchaseAsync(
            CreatePurchaseRequest request)
        {
            var employeeExists =
                await _employeeRepository.ExistsAsync(request.EmployeeId);

            if (!employeeExists)
            {
                throw new InvalidOperationException(
                    "Employee not found.");
            }

            var product =
                await _productRepository
                    .GetActiveByItemCodeAsync(request.ItemCode);

            if (product == null)
            {
                throw new InvalidOperationException(
                    "Product not found or inactive.");
            }

            var purchase = new Purchase
            {
                EmployeeId = request.EmployeeId,

                ItemCode = product.ItemCode,

                Description = product.Name,

                Amount = product.Price,

                Currency = product.Currency,

                Status = PurchaseStatus.Pending,

                CreatedAtUtc = DateTime.UtcNow,

                UpdatedAtUtc = DateTime.UtcNow
            };

            return await _purchaseRepository.AddAsync(purchase);
        }


    }
}
