using EnterpriseEmployeeSystem.Api.DTOs;
using EnterpriseEmployeeSystem.Api.Services.Purchases;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseEmployeeSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchasesController : ControllerBase
    {
       
        private readonly IPurchaseService _purchaseService;

        public PurchasesController(
            IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchase(
            CreatePurchaseRequest request)
        {
            try
            {
                var purchase =
                    await _purchaseService
                        .CreatePurchaseAsync(request);

                return Ok(purchase);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

    }
}
