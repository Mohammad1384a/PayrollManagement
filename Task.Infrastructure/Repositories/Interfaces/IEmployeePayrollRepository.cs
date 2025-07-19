using TaskType = System.Threading.Tasks.Task;
using Task.Infrastructure.Model;

namespace Task.Infrastructure.Repositories.Interfaces;
public interface IEmployeePayrollRepository {
    TaskType AddAsync(EmployeePayRoll payroll);
    TaskType UpdateAsync(EmployeePayRoll payroll);
    TaskType DeleteAsync(int id);
    Task<EmployeePayRoll?> GetByIdAsync(int id);
    Task<IEnumerable<EmployeePayRoll>?> GetByEmpIdAsync(int id);
    Task<IEnumerable<EmployeePayRoll>?> GetByDateRangeForEmpAsync(int empId,DateTime from, DateTime to);
}
