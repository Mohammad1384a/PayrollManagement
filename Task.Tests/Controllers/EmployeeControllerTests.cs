using Microsoft.AspNetCore.Mvc;
using Moq;
using Task.API.Controllers;
using Task.Core.DTOs;
using Task.Core.Services.Interfaces;
using Task.Infrastructure.Model;
using TaskType = System.Threading.Tasks.Task;

namespace Task.Tests.Controllers;

public class EmployeeControllerTests {
    [Fact]
    public async TaskType AddEmployee_Success_ReturnsOk() {
        var serviceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeController(serviceMock.Object);
        var dto = new CreateEmployeeDTO("A", "B");

        var result = await controller.AddEmployee(dto);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async TaskType AddEmployee_ServiceThrows_ReturnsBadRequest() {
        var serviceMock = new Mock<IEmployeeService>();
        serviceMock.Setup(s => s.AddEmployeeAsync(It.IsAny<CreateEmployeeDTO>()))
            .ThrowsAsync(new Exception("err"));
        var controller = new EmployeeController(serviceMock.Object);

        var result = await controller.AddEmployee(new CreateEmployeeDTO("a", "b"));

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async TaskType GetAllAsync_ReturnsOkWithData() {
        var serviceMock = new Mock<IEmployeeService>();
        serviceMock.Setup(s => s.GetAllAsync())
            .ReturnsAsync(new List<Employee> { new Employee() });
        var controller = new EmployeeController(serviceMock.Object);

        var result = await controller.GetAllAsync();

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async TaskType GetAllAsync_ServiceThrows_ReturnsBadRequest() {
        var serviceMock = new Mock<IEmployeeService>();
        serviceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("err"));
        var controller = new EmployeeController(serviceMock.Object);

        var result = await controller.GetAllAsync();

        Assert.IsType<BadRequestObjectResult>(result);
    }
}