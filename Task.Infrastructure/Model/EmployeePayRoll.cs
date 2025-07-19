
namespace Task.Infrastructure.Model;
public class EmployeePayRoll {
    public int Id { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal Allowance { get; set; }
    public decimal Transportation { get; set; }
    public decimal TotalSalary { get; set; }
    public byte OverTimeCalc { get; set; }
    public DateTime Date { get; set; }
    public int EmployeeId { get; set; }
    public virtual Employee Employee { get; set; } = null!;
}
