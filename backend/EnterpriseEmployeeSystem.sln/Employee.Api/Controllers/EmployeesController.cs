using EnterpriseEmployeeSystem.Api.Models;
using EnterpriseEmployeeSystem.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseEmployeeSystem.Api.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetEmployees()
        {
            var employees = await _employeeService.GetEmployees();

            return Ok(employees);
        }

        [HttpPost]
        public async Task<ActionResult> AddEmployee(Employee  employee)
        {
              await _employeeService.AddAsync(employee);

            return Ok();
        }
    }
}
