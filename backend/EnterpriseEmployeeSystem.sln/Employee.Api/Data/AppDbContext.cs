using EnterpriseEmployeeSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseEmployeeSystem.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; } = null!;
    
    }
}
