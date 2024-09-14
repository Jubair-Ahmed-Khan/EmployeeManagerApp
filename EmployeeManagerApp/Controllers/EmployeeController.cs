using EmployeeManagerApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagerApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDbContext _context;

        public EmployeeController(EmployeeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var employees = _context.Employees.ToList();
            return View(employees);
        }
    }
}
