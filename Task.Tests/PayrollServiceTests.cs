using AutoMapper;
using Moq;
using Task.Core.DTOs;
using Task.Core.Services;
using Task.Infrastructure.Model;
using Task.Infrastructure.Repositories.Interfaces;

namespace Task.Tests;

public class PayrollServiceTests
{
    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreatePayrollDto, EmployeePayRoll>();
            cfg.CreateMap<UpdatePayrollDto, EmployeePayRoll>();
        });
        return config.CreateMapper();
    }

    [Fact]
    public async Task AddPayrollAsync_ValidDto_AddsPayrollWithCalculatedTotal()
    {
        // Arrange
        var mapper = CreateMapper();
        var payrollRepoMock = new Mock<IEmployeePayrollRepository>();
        var empRepoMock = new Mock<IEmployeeRepository>();
        empRepoMock.Setup(r => r.GetByIdAsync(1))
                   .ReturnsAsync(new Employee { Id = 1, FirstName = "John", LastName = "Doe" });

        var service = new PayrollService(payrollRepoMock.Object, mapper, empRepoMock.Object);

        var dto = new CreatePayrollDto(
            EmployeeId: 1,
            BasicSalary: 100m,
            Allowance: 50m,
            Transportation: 25m,
            TotalSalary: 0m,
            OverTimeCalc: 1);

        // Act
        await service.AddPayrollAsync(dto);

        // Assert
        payrollRepoMock.Verify(r => r.AddAsync(It.Is<EmployeePayRoll>(p =>
            p.EmployeeId == 1 &&
            p.BasicSalary == 100m &&
            p.Allowance == 50m &&
            p.Transportation == 25m &&
            p.TotalSalary == (100m + 50m + 25m + (100m + 50m) * 2)
        )), Times.Once);
    }

    [Fact]
    public async Task AddPayrollAsync_EmployeeNotFound_ThrowsException()
    {
        var mapper = CreateMapper();
        var payrollRepoMock = new Mock<IEmployeePayrollRepository>();
        var empRepoMock = new Mock<IEmployeeRepository>();
        empRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                   .ReturnsAsync((Employee?)null);

        var service = new PayrollService(payrollRepoMock.Object, mapper, empRepoMock.Object);

        var dto = new CreatePayrollDto(1, 1m, 1m, 1m, 0m, 1);

        await Assert.ThrowsAsync<Exception>(() => service.AddPayrollAsync(dto));
    }

    [Fact]
    public async Task AddPayrollAsync_InvalidOvertimeCalc_ThrowsException()
    {
        var mapper = CreateMapper();
        var payrollRepoMock = new Mock<IEmployeePayrollRepository>();
        var empRepoMock = new Mock<IEmployeeRepository>();
        empRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                   .ReturnsAsync(new Employee());

        var service = new PayrollService(payrollRepoMock.Object, mapper, empRepoMock.Object);

        var dto = new CreatePayrollDto(1, 1m, 1m, 1m, 0m, 5);

        await Assert.ThrowsAsync<Exception>(() => service.AddPayrollAsync(dto));
    }
}
