using LastDance.Areas.Admin.ViewModels.Employee;
using LastDance.DAL;
using LastDance.Models;
using LastDance.Utils;
using LastDance.Utils.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LastDance.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(AppDbContext context, IWebHostEnvironment env)
        {
             _context = context;
             _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<GetEmployeeAdminVM> employeeAdminVMs = await _context.Employees
                .Include(e=>e.Department)
                .Select(e=>new GetEmployeeAdminVM
                {
                    Name = e.Name,
                    Surname = e.Surname,
                    DepartmentName = e.Department.Name,


                }).ToListAsync();

            return View(employeeAdminVMs);
        }

        public async Task<IActionResult> Create()
        {
            CreateEmployeeVM employeeVM = new()
            {
                Departments = await _context.Departments.ToListAsync(),

            };
            return View(employeeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVM employeeVM)
        {
            employeeVM.Departments = await _context.Departments.ToListAsync() ?? new List<Department>();

            //if (!ModelState.IsValid)
            //    return View(employeeVM);

            if (!await _context.Departments.AnyAsync(d => d.Id == employeeVM.DepartmentId))
            {
                ModelState.AddModelError(nameof(employeeVM.DepartmentId), "Department does not exist");
                return View(employeeVM);
            }


            if (!employeeVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(employeeVM.Photo), "Photo type is invalid");
                return View(employeeVM);
            }

            if (!employeeVM.Photo.ValidateSize(SizeEnum.Mb, 2))
            {
                ModelState.AddModelError(nameof(employeeVM.Photo), "Photo size is invalid");
                return View(employeeVM);
            }

              

           
                Employee employee = new()
                {
                    DepartmentId = employeeVM.DepartmentId,
                    Name = employeeVM.Name,
                    Surname = employeeVM.Surname,
                    Image = await employeeVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img")
                };

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            
      
        }



    }

}

