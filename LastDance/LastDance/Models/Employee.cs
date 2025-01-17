namespace LastDance.Models
{
    public class Employee : BaseEntity
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
