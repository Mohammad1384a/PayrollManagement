namespace Task.Infrastructure.Model;

public class Employee {
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public virtual ICollection<EmployeePayRoll> Payrolls { get; set; } = new List<EmployeePayRoll>();
}