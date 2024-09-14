using EmployeeManagerApp.Models;
using EmployeeManagerApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagerApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public EmployeeController(EmployeeDbContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult Index()
        {
            var employees = _context.Employees.ToList();
            return View(employees);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeDto employeeDto)
        {
            if (employeeDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            if(!ModelState.IsValid)
            {
                return View(employeeDto); 
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(employeeDto.ImageFile!.FileName);

            string imageFullPath = _environment.WebRootPath + "/profiles/" + newFileName;

            using (var stream = System.IO.File.Create(imageFullPath))
            {
                employeeDto.ImageFile.CopyTo(stream);
            }

           

            Employee employee = new Employee()
            {
                FullName = employeeDto.FullName,
                Email= employeeDto.Email,
                Mobile = employeeDto.Mobile,
                DateOfBirth = employeeDto.DateOfBirth,
                ImageFileName = newFileName
            };

            _context.Employees.Add(employee);
            _context.SaveChanges();

            return RedirectToAction("Index", "Employee");

            
        }
    }
}
