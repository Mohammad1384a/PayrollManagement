using TaskType = System.Threading.Tasks.Task;
using AutoMapper;
using Moq;
using Task.Core.DTOs;
using Task.Core.Services;
using Task.Infrastructure.Model;
using Task.Infrastructure.Repositories.Interfaces;

namespace Task.Tests.Services;

public class EmployeeServiceTests {
    private static IMapper CreateMapper() {
        var config = new MapperConfiguration(cfg => {
            cfg.CreateMap<CreateEmployeeDTO, Employee>();
        });
        return config.CreateMapper();
    }

    [Fact]
    public async TaskType AddEmployeeAsync_ValidDto_CallsRepository() {
        var mapper = CreateMapper();
        var repoMock = new Mock<IEmployeeRepository>();
        var service = new EmployeeService(mapper, repoMock.Object);

        var dto = new CreateEmployeeDTO("John", "Doe");
        await service.AddEmployeeAsync(dto);

        repoMock.Verify(r => r.AddAsync(It.Is<Employee>(e =>
            e.FirstName == "John" && e.LastName == "Doe")), Times.Once);
    }

    [Fact]
    public async TaskType GetAllAsync_ReturnsEmployees() {
        var mapper = CreateMapper();
        var repoMock = new Mock<IEmployeeRepository>();
        repoMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Employee> { new() { Id = 1, FirstName = "A", LastName = "B" } });

        var service = new EmployeeService(mapper, repoMock.Object);

        var result = await service.GetAllAsync();

        Assert.Single(result);
    }
}
