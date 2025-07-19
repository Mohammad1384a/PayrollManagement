using Task.Infrastructure.Model;
using TaskType = System.Threading.Tasks.Task;

namespace Task.Infrastructure.Repositories.Interfaces;
public interface IEmployeeRepository {
    TaskType AddAsync(Employee emp);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
}
