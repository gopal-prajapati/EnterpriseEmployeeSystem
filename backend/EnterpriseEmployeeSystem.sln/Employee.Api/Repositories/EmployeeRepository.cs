using EnterpriseEmployeeSystem.Api.Data;
using EnterpriseEmployeeSystem.Api.Models;
using EnterpriseEmployeeSystem.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseEmployeeSystem.Api.Repositories
{
    public class EmployeeRepository: IEmployeeRepository
    {
        private readonly AppDbContext _context;
        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task AddAsync(Employee employee)
        {
            try
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public async Task<bool> ExistsAsync(int employeeId)
        {
            return await _context.Employees
                .AnyAsync(x => x.Id == employeeId);
        }

    }
}
