using EnterpriseEmployeeSystem.Api.Repositories.Products;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseEmployeeSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("{itemCode}")]
        public async Task<IActionResult> GetByItemCode(string itemCode)
        {
            var product =
                await _productRepository.GetActiveByItemCodeAsync(itemCode);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found or inactive."
                });
            }

            return Ok(product);
        }

    }
}
