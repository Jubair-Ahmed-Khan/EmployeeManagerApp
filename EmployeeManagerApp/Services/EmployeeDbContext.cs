using EmployeeManagerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagerApp.Services
{
    public class EmployeeDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {

        }

    }
}
