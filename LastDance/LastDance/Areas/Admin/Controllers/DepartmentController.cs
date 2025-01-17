using LastDance.Areas.Admin.ViewModels.Department;
using LastDance.DAL;
using LastDance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LastDance.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<GetDepartmentAdminVM> departments = await _context.Departments.Include(d => d.Employees)
               .Select(d => new GetDepartmentAdminVM()
               {
                   Name = d.Name,
                   EmployeeCount = d.Employees.Count
               }).ToListAsync();
            return View(departments);
        }

        public  IActionResult Create()
        {
           

            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateDepartmentVM departmentVM)
        {
            if (!ModelState.IsValid)
                return View();
            bool result = await _context.Departments.AnyAsync(d=>d.Name.Trim() == departmentVM.Name.Trim());

            if(result)
            {
                ModelState.AddModelError("Name", "this department already exists");
                return View(departmentVM);
            }
               
            Department department = new Department()
            {
                Name = departmentVM.Name,

            };

            await _context.AddAsync(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index),"Home", new { Area = "" });

        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            bool existed = await _context.Departments.AnyAsync(d => d.Id == id);
            if (!existed) return NotFound();
            return View();

        }

        [HttpPost]

        public async Task<IActionResult> Update(int id, UpdateDepartmentVM departmentVM)
        {
            if ( id < 1) return BadRequest();

            bool result = await _context.Departments.AnyAsync(d => d.Id == id);
            if (!result) return NotFound();

           
            var existed = await _context.Departments.FirstOrDefaultAsync(d => d.Id == departmentVM.Id);
            existed.Name = departmentVM.Name;

            _context.Departments.Update(existed);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Home",new {Area=""});


        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id<1||id is null)
                return BadRequest();

            Department? department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id); 
            if (department is null) return NotFound();

            _context.Departments.Remove(department);
             await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Home");
           


        }
        

    }
}
