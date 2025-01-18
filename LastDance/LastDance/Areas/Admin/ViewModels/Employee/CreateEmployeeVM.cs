using LastDance.Models;
using System.ComponentModel.DataAnnotations;

namespace LastDance.Areas.Admin.ViewModels.Employee
{
    public class CreateEmployeeVM
    {

        
        public IFormFile Photo { get; set; }
        
        public string Name { get; set; }

        [Required]
        public List<Models.Department>? Departments { get; set; }
        public int DepartmentId { get; set; }
        
        public string Surname { get; set; }
 
    }
}
