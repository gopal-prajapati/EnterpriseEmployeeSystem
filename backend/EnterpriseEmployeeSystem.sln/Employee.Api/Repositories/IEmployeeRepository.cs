using EnterpriseEmployeeSystem.Api.Models;

namespace EnterpriseEmployeeSystem.Api.Repositories
{
    public interface IEmployeeRepository
    {
       Task<List<Employee>> GetAllAsync();
        Task AddAsync(Employee employee);
    }
}
