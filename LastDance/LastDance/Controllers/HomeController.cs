using System.Diagnostics;
using LastDance.Models;
using Microsoft.AspNetCore.Mvc;

namespace LastDance.Controllers
{
    public class HomeController : Controller
    {
  

        public IActionResult Index()
        {
            return View();
        }

     

    
    }
}
