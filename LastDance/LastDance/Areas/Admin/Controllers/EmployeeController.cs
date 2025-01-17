using LastDance.Areas.Admin.ViewModels.Employee;
using LastDance.DAL;
using LastDance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LastDance.Areas.Admin.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
             _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(CreateEmployeeVM employeeVM)
        //{
        //    if(!ModelState.IsValid) 
        //        return View(employeeVM);




           
         

            
        //}



    }

 }

