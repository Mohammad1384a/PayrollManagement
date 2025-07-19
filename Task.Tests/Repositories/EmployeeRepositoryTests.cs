using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Task.Infrastructure.Data;
using Task.Infrastructure.Model;
using Task.Infrastructure.Repositories;
using TaskType = System.Threading.Tasks.Task;

namespace Task.Tests.Repositories;

public class EmployeeRepositoryTests {
    private static (TaskDbContext, IConfiguration) CreateContext() {
        var options = new DbContextOptionsBuilder<TaskDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new TaskDbContext(options);
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = "fake" })
            .Build();
        return (context, config);
    }

    [Fact]
    public async TaskType AddAsync_AddsEmployee() {
        var (context, config) = CreateContext();
        var repo = new EmployeeRepository(context, config);
        var emp = new Employee { FirstName = "A", LastName = "B" };

        await repo.AddAsync(emp);

        Assert.Equal(1, await context.Employees.CountAsync());
    }

    [Fact]
    public async TaskType GetByIdAsync_InvalidConnection_Throws() {
        var (context, config) = CreateContext();
        var repo = new EmployeeRepository(context, config);

        await Assert.ThrowsAnyAsync<Exception>(() => repo.GetByIdAsync(1));
    }

    [Fact]
    public async TaskType GetAllAsync_InvalidConnection_Throws() {
        var (context, config) = CreateContext();
        var repo = new EmployeeRepository(context, config);

        await Assert.ThrowsAnyAsync<Exception>(() => repo.GetAllAsync());
    }
}