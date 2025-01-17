namespace LastDance.Areas.Admin.ViewModels.Employee
{
    public class CreateEmployeeVM
    {
        public IFormFile Photo { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Surname { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
    }
}
