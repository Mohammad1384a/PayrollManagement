using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Task.Infrastructure.Data;
using Task.Infrastructure.Model;
using Task.Infrastructure.Repositories;
using TaskType = System.Threading.Tasks.Task;

namespace Task.Tests.Repositories;

public class EmployeePayrollRepositoryTests {
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
    public async TaskType AddAsync_AddsPayroll() {
        var (context, config) = CreateContext();
        var repo = new EmployeePayrollRepository(context, config);
        var payroll = new EmployeePayRoll {
            EmployeeId = 1,
            BasicSalary = 10m,
            Allowance = 5m,
            Transportation = 2m,
            OverTimeCalc = 1,
            Date = DateTime.Today
        };

        await repo.AddAsync(payroll);

        Assert.Equal(1, await context.EmployeePayrolls.CountAsync());
    }


    [Fact]
    public async TaskType GetByIdAsync_InvalidConnection_Throws() {
        var (context, config) = CreateContext();
        var repo = new EmployeePayrollRepository(context, config);

        await Assert.ThrowsAnyAsync<Exception>(() => repo.GetByIdAsync(1));
    }

    [Fact]
    public async TaskType GetByEmpIdAsync_InvalidConnection_Throws() {
        var (context, config) = CreateContext();
        var repo = new EmployeePayrollRepository(context, config);

        await Assert.ThrowsAnyAsync<Exception>(() => repo.GetByEmpIdAsync(1));
    }

    [Fact]
    public async TaskType GetByDateRangeForEmpAsync_InvalidConnection_Throws() {
        var (context, config) = CreateContext();
        var repo = new EmployeePayrollRepository(context, config);

        await Assert.ThrowsAnyAsync<Exception>(() => repo.GetByDateRangeForEmpAsync(1, DateTime.Today, DateTime.Today));
    }

    [Fact]
    public async TaskType UpdateAsync_UpdatesPayroll() {
        var (context, config) = CreateContext();
        var repo = new EmployeePayrollRepository(context, config);
        var payroll = new EmployeePayRoll {
            EmployeeId = 1,
            BasicSalary = 10m,
            Allowance = 5m,
            Transportation = 2m,
            OverTimeCalc = 1,
            Date = DateTime.Today
        };
        context.EmployeePayrolls.Add(payroll);
        await context.SaveChangesAsync();

        payroll.BasicSalary = 20m;
        await repo.UpdateAsync(payroll);

        Assert.Equal(20m, (await context.EmployeePayrolls.FirstAsync()).BasicSalary);
    }

    [Fact]
    public async TaskType DeleteAsync_RemovesPayroll() {
        var (context, config) = CreateContext();
        var repo = new EmployeePayrollRepository(context, config);
        var payroll = new EmployeePayRoll { EmployeeId = 1, BasicSalary = 1m, Allowance = 1m, Transportation = 1m, OverTimeCalc = 1, Date = DateTime.Today };
        context.EmployeePayrolls.Add(payroll);
        await context.SaveChangesAsync();

        await repo.DeleteAsync(payroll.Id);

        Assert.Empty(context.EmployeePayrolls);
    }
}