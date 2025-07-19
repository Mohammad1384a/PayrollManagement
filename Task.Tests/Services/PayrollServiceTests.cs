using TaskType = System.Threading.Tasks.Task;
using AutoMapper;
using Moq;
using Task.Core.DTOs;
using Task.Core.Services;
using Task.Infrastructure.Model;
using Task.Infrastructure.Repositories.Interfaces;

namespace Task.Tests;

public class PayrollServiceTests {
    private static IMapper CreateMapper() {
        var config = new MapperConfiguration(cfg => {
            cfg.CreateMap<CreatePayrollDto, EmployeePayRoll>();
            cfg.CreateMap<UpdatePayrollDto, EmployeePayRoll>();
        });
        return config.CreateMapper();
    }

    [Fact]
    public async TaskType AddPayrollAsync_ValidDto_AddsPayrollWithCalculatedTotal() {
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
    public async TaskType AddPayrollAsync_EmployeeNotFound_ThrowsException() {
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
    public async TaskType AddPayrollAsync_InvalidOvertimeCalc_ThrowsException() {
        var mapper = CreateMapper();
        var payrollRepoMock = new Mock<IEmployeePayrollRepository>();
        var empRepoMock = new Mock<IEmployeeRepository>();
        empRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                   .ReturnsAsync(new Employee());

        var service = new PayrollService(payrollRepoMock.Object, mapper, empRepoMock.Object);

        var dto = new CreatePayrollDto(1, 1m, 1m, 1m, 0m, 5);

        await Assert.ThrowsAsync<Exception>(() => service.AddPayrollAsync(dto));
    }

    [Fact]
    public async TaskType UpdatePayrollAsync_ValidDto_UpdatesPayroll() {
        var mapper = CreateMapper();
        var payrollRepoMock = new Mock<IEmployeePayrollRepository>();
        var empRepoMock = new Mock<IEmployeeRepository>();
        var payroll = new EmployeePayRoll { Id = 1, BasicSalary = 100m, Allowance = 50m, Transportation = 20m, OverTimeCalc = 1 };
        payrollRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(payroll);
        var service = new PayrollService(payrollRepoMock.Object, mapper, empRepoMock.Object);

        var dto = new UpdatePayrollDto(1, 200m, 60m, 30m, 0m, 2);

        await service.UpdatePayrollAsync(dto);

        payrollRepoMock.Verify(r => r.UpdateAsync(It.Is<EmployeePayRoll>(p =>
            p.BasicSalary == 200m &&
            p.Allowance == 60m &&
            p.Transportation == 30m &&
            p.TotalSalary == (200m + 60m + 30m + (200m + 60m) * 3)
        )), Times.Once);
    }

    [Fact]
    public async TaskType UpdatePayrollAsync_NotFound_ThrowsException() {
        var mapper = CreateMapper();
        var payrollRepoMock = new Mock<IEmployeePayrollRepository>();
        payrollRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((EmployeePayRoll?)null);
        var empRepoMock = new Mock<IEmployeeRepository>();
        var service = new PayrollService(payrollRepoMock.Object, mapper, empRepoMock.Object);

        var dto = new UpdatePayrollDto(1, 1m, 1m, 1m, 0m, 1);

        await Assert.ThrowsAsync<Exception>(() => service.UpdatePayrollAsync(dto));
    }

    [Fact]
    public async TaskType UpdatePayrollAsync_InvalidOvertimeCalc_ThrowsException() {
        var mapper = CreateMapper();
        var payrollRepoMock = new Mock<IEmployeePayrollRepository>();
        payrollRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new EmployeePayRoll());
        var empRepoMock = new Mock<IEmployeeRepository>();
        var service = new PayrollService(payrollRepoMock.Object, mapper, empRepoMock.Object);

        var dto = new UpdatePayrollDto(1, 1m, 1m, 1m, 0m, 5);

        await Assert.ThrowsAsync<Exception>(() => service.UpdatePayrollAsync(dto));
    }

    [Fact]
    public async TaskType DeletePayrollAsync_ValidId_DeletesPayroll() {
        var mapper = CreateMapper();
        var payrollRepoMock = new Mock<IEmployeePayrollRepository>();
        payrollRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new EmployeePayRoll { Id = 1 });
        var empRepoMock = new Mock<IEmployeeRepository>();
        var service = new PayrollService(payrollRepoMock.Object, mapper, empRepoMock.Object);

        await service.DeletePayrollAsync(1);

        payrollRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async TaskType DeletePayrollAsync_NotFound_ThrowsException() {
        var mapper = CreateMapper();
        var payrollRepoMock = new Mock<IEmployeePayrollRepository>();
        payrollRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((EmployeePayRoll?)null);
        var empRepoMock = new Mock<IEmployeeRepository>();
        var service = new PayrollService(payrollRepoMock.Object, mapper, empRepoMock.Object);

        await Assert.ThrowsAsync<Exception>(() => service.DeletePayrollAsync(1));
    }

    private static IMapper CreateFullMapper() {
        var config = new MapperConfiguration(cfg => {
            cfg.CreateMap<CreatePayrollDto, EmployeePayRoll>();
            cfg.CreateMap<UpdatePayrollDto, EmployeePayRoll>();
            cfg.CreateMap<EmployeePayRoll, PayrollDTO>();
        });
        return config.CreateMapper();
    }

    [Fact]
    public async TaskType GetPayrollByEmpIdAsync_ReturnsMappedDtos() {
        var mapper = CreateFullMapper();
        var payrollRepoMock = new Mock<IEmployeePayrollRepository>();
        var empRepoMock = new Mock<IEmployeeRepository>();
        empRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Employee());
        payrollRepoMock.Setup(r => r.GetByEmpIdAsync(1))
            .ReturnsAsync(new List<EmployeePayRoll> { new() { Id = 2, BasicSalary = 1m, Allowance = 1m, Transportation = 1m, OverTimeCalc = 1, Date = DateTime.Today } });
        var service = new PayrollService(payrollRepoMock.Object, mapper, empRepoMock.Object);

        var result = await service.GetPayrollByEmpIdAsync(1);

        Assert.Single(result!);
    }

    [Fact]
    public async TaskType GetPayrollByEmpIdAsync_EmployeeNotFound_ThrowsException() {
        var mapper = CreateFullMapper();
        var payrollRepoMock = new Mock<IEmployeePayrollRepository>();
        var empRepoMock = new Mock<IEmployeeRepository>();
        empRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Employee?)null);
        var service = new PayrollService(payrollRepoMock.Object, mapper, empRepoMock.Object);

        await Assert.ThrowsAsync<Exception>(() => service.GetPayrollByEmpIdAsync(1));
    }

    [Fact]
    public async TaskType GetEmpPayrollsByDateAsync_ReturnsDtos() {
        var mapper = CreateFullMapper();
        var payrollRepoMock = new Mock<IEmployeePayrollRepository>();
        var empRepoMock = new Mock<IEmployeeRepository>();
        empRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Employee());
        payrollRepoMock.Setup(r => r.GetByDateRangeForEmpAsync(1, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<EmployeePayRoll> { new() { Id = 3, BasicSalary = 1m, Allowance = 1m, Transportation = 1m, OverTimeCalc = 1, Date = DateTime.Today } });
        var service = new PayrollService(payrollRepoMock.Object, mapper, empRepoMock.Object);

        var result = await service.GetEmpPayrollsByDateAsync(1, DateTime.Today, DateTime.Today);

        Assert.Single(result);
    }

    [Fact]
    public async TaskType GetEmpPayrollsByDateAsync_EmployeeNotFound_ThrowsException() {
        var mapper = CreateFullMapper();
        var payrollRepoMock = new Mock<IEmployeePayrollRepository>();
        var empRepoMock = new Mock<IEmployeeRepository>();
        empRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Employee?)null);
        var service = new PayrollService(payrollRepoMock.Object, mapper, empRepoMock.Object);

        await Assert.ThrowsAsync<Exception>(() => service.GetEmpPayrollsByDateAsync(1, DateTime.Today, DateTime.Today));
    }
}
