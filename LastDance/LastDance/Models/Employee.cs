namespace LastDance.Models
{
    public class Employee : BaseEntity
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

     


        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
