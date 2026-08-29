using EnterpriseEmployeeSystem.Api.DTOs;
using EnterpriseEmployeeSystem.Api.Models;

namespace EnterpriseEmployeeSystem.Api.Services.Payments
{
    public interface IPaymentService
    {
        Task<Payment> CreatePaymentAsync(CreatePaymentRequest request);

    }
}
