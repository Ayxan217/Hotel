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
                    Image = e.Image,



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



        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 0)
                return BadRequest();
                       

            Employee employee = await _context.Employees.Include(e=>e.Department)
                .FirstOrDefaultAsync(e=>e.Id == id);


            if (employee is null)
                return NotFound();

            UpdateEmployeeVM employeeVM = new()
            {
                Name = employee.Name,
                Surname = employee.Surname,
                DepartmentId = employee.DepartmentId,
                Departments = await _context.Departments.ToListAsync(),
                Image = employee.Image,

            };

            return View(employeeVM);
        }

        [HttpPost]

        public async Task<IActionResult> Update(int? id, UpdateEmployeeVM employeeVM)
        {
            if(id is null || id < 0)   
                return BadRequest();
            Employee employee = await _context.Employees.Include(e=> e.Department).FirstOrDefaultAsync(e=>e.Id == id);
            if(employee is null)
                return NotFound();
            employeeVM.Departments = await _context.Departments.ToListAsync();
            employeeVM.Image = employee.Image;


            if(employeeVM.Photo != null)
            {
                if (!employeeVM.Photo.ValidateSize(SizeEnum.Mb, 2))
                {
                    ModelState.AddModelError(nameof(employeeVM.Photo), "invalid");
                    return View(employeeVM);
                }
                if (!employeeVM.Photo.ValidateType("image"))
                {
                    ModelState.AddModelError(nameof(employeeVM.Photo), "invalid");
                    return View(employeeVM);
                }
            }

            if(employeeVM.DepartmentId != employee.DepartmentId)
            {
                bool result = employeeVM.Departments.Any(e => e.Id == employeeVM.DepartmentId);
                if(!result)
                    return View(employeeVM);
            }

            if(employeeVM.Photo != null)
            {
                string fileName = await employeeVM.Photo.CreateFileAsync(_env.WebRootPath,"assets", "img");
                employee.Image.DeleteFile(_env.WebRootPath,"assets","img");
                employeeVM.Image = fileName;
                


                

            }

            employee.Name = employeeVM.Name;
            employee.Surname = employeeVM.Surname;
            employee.DepartmentId = employeeVM.DepartmentId;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");




        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            Employee employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if(employee == null)
                return NotFound();

            employee.Image.DeleteFile(_env.WebRootPath, "assets", "img");
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }

    }

}

