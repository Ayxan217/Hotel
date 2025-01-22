using System.Diagnostics;
using LastDance.DAL;
using LastDance.Models;
using LastDance.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LastDance.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async  Task<IActionResult> Index()
        {
            HomeVM homeVM = new()
            {
                Employees = await _context.Employees.Take(4).ToListAsync(),
                Departments = await _context.Departments.ToListAsync(),
            };
            return View(homeVM);
        }

     

    
    }
}
