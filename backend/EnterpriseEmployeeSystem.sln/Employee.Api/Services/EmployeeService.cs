using EnterpriseEmployeeSystem.Api.Models;
using EnterpriseEmployeeSystem.Api.Repositories;

namespace EnterpriseEmployeeSystem.Api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public async Task<List<Employee>> GetEmployees()
        {
            return await _employeeRepository.GetAllAsync();
        }


        public async Task AddAsync(Employee employee)
        {
            await _employeeRepository.AddAsync(employee);
        }

    }
}
