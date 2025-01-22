using LastDance.Models;
using System.ComponentModel.DataAnnotations;

namespace LastDance.Areas.Admin.ViewModels.Employee
{
    public class UpdateEmployeeVM
    {
        public int Id { get; set; }
        
        public IFormFile? Photo { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public List<Models.Department>? Departments { get; set; }
        public string? Image {  get; set; }
        public Models.Department Department { get; set; }
        public int DepartmentId { get; set; }
    }
}
