using Task.Core.DTOs;
using Task.Infrastructure.Model;
using TaskType = System.Threading.Tasks.Task;

namespace Task.Core.Services.Interfaces;
public interface IEmployeeService {
    TaskType AddEmployeeAsync(CreateEmployeeDTO dto);
    Task<IEnumerable<Employee>> GetAllAsync();
}
