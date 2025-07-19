using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Task.Infrastructure.Data;
using Task.Infrastructure.Model;
using Task.Infrastructure.Repositories.Interfaces;
using TaskType = System.Threading.Tasks.Task;

namespace Task.Infrastructure.Repositories;
public class EmployeeRepository(TaskDbContext context, IConfiguration configuration) : IEmployeeRepository{
    private readonly TaskDbContext _context = context;
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    public async TaskType AddAsync(Employee emp) {
        await _context.Set<Employee>().AddAsync(emp);
        await _context.SaveChangesAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id) {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<Employee>("select * from Employees where Id = @Id", new {Id = id});
    }
 
    public async Task<IEnumerable<Employee>> GetAllAsync() {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<Employee>("select * from Employees");
    }

}
