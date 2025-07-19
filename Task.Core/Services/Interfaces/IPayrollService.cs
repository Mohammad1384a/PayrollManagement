using Task.Core.DTOs;
using TaskType = System.Threading.Tasks.Task;

namespace Task.Core.Services.Interfaces;
public interface IPayrollService {
    TaskType AddPayrollAsync(CreatePayrollDto dto);
    TaskType UpdatePayrollAsync(UpdatePayrollDto dto);
    TaskType DeletePayrollAsync(int id);
    Task<IEnumerable<PayrollDTO>?> GetPayrollByEmpIdAsync(int empId);
    Task<IEnumerable<PayrollDTO>> GetEmpPayrollsByDateAsync(int empId,DateTime from, DateTime to);
}
