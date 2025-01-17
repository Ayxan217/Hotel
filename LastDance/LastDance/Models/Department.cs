
namespace LastDance.Models
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
