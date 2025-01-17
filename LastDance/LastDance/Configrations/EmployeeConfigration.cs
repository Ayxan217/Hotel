using LastDance.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LastDance.Configrations
{
    public class EmployeeConfigration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasColumnType("VARCHAR(100)");
        }
    }
}
