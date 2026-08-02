
using EnterpriseEmployeeSystem.Api.Models;

namespace EnterpriseEmployeeSystem.Api.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetEmployees();
        Task AddAsync(Employee employee);

    }
}
