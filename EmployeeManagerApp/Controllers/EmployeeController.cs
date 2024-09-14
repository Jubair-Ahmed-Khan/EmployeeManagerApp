﻿using EmployeeManagerApp.Models;
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

        public IActionResult Edit(int id) 
        {
            var employee = _context.Employees.Find(id);

            if(employee == null)
            {
                return RedirectToAction("Index", "Employee");
            }

            var employeeDto = new EmployeeDto()
            {
                FullName = employee.FullName,
                Email = employee.Email,
                Mobile = employee.Mobile,
                DateOfBirth = employee.DateOfBirth
            };

            ViewData["EmployeeId"] = employee.Id;
            ViewData["ImageFileName"] = employee.ImageFileName;

            return View(employeeDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, EmployeeDto employeeDto)
        {
            var employee = _context.Employees.Find(id);

            if(employee == null)
            {
                return RedirectToAction("Index", "Employee");
            }

            if(!ModelState.IsValid)
            {
                ViewData["EmployeeId"] = employee.Id;
                ViewData["ImageFileName"] = employee.ImageFileName;

                return View(employeeDto);
            }

            string newFileName = employee.ImageFileName;

            if(employeeDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(employeeDto.ImageFile.FileName);

                string imageFullPath = _environment.WebRootPath + "/profiles/" + newFileName;

                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    employeeDto.ImageFile.CopyTo(stream);
                }

                string oldImageFullPath = _environment.WebRootPath + "/profiles/" + employee.ImageFileName;

                System.IO.File.Delete(oldImageFullPath);
            }

            employee.FullName = employeeDto.FullName;
            employee.Email = employeeDto.Email;
            employee.Mobile = employeeDto.Mobile;
            employee.DateOfBirth = employeeDto.DateOfBirth;
            employee.ImageFileName = newFileName;

            _context.SaveChanges();

            return RedirectToAction("Index", "Employee");
        }
    }
}
