using Task.Infrastructure.Data;
using Task.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TaskType = System.Threading.Tasks.Task;
using Task.Infrastructure.Model;


namespace Task.Infrastructure.Repositories;

public class EmployeePayrollRepository(TaskDbContext context, IConfiguration configuration) : IEmployeePayrollRepository {
    private readonly TaskDbContext _context = context;
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async TaskType AddAsync(EmployeePayRoll payroll) {
        await _context.EmployeePayrolls.AddAsync(payroll);
        await _context.SaveChangesAsync();
    }

    public async TaskType UpdateAsync(EmployeePayRoll payroll) {
        _context.EmployeePayrolls.Update(payroll);
        await _context.SaveChangesAsync();
    }

    public async TaskType DeleteAsync(int id) {
        var entity = await _context.EmployeePayrolls.FindAsync(id);
        if (entity != null) {
            _context.EmployeePayrolls.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<EmployeePayRoll?> GetByIdAsync(int id) {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<EmployeePayRoll>(
            "select * from EmployeePayrolls where Id = @Id", new { Id = id });
    }

    public async Task<IEnumerable<EmployeePayRoll>?> GetByEmpIdAsync(int empId) {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<EmployeePayRoll>(
            "select ep.Id,ep.BasicSalary,ep.Allowance,ep.Transportation,ep.Date from Employees e join EmployeePayrolls ep on ep.EmployeeId = e.Id where e.Id = @Id", new { Id = empId });
    }

    public async Task<IEnumerable<EmployeePayRoll>?> GetByDateRangeForEmpAsync(int empId, DateTime from, DateTime to) {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<EmployeePayRoll>(
            "select ep.Id,ep.BasicSalary,ep.Allowance,ep.Transportation,ep.Date from Employees e join EmployeePayrolls ep on ep.EmployeeId = e.Id WHERE Date BETWEEN @From AND @To and e.Id = @Id",
            new { From = from, To = to, Id = empId });
    }
}
