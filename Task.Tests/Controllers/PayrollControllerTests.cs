using Microsoft.AspNetCore.Mvc;
using Moq;
using Task.API.Controllers;
using Task.Core.DTOs;
using Task.Core.Services.Interfaces;

namespace Task.Tests.Controllers;

public class PayrollControllerTests
{
    [Fact]
    public async Task AddPayrollAsync_Success_ReturnsOk()
    {
        var serviceMock = new Mock<IPayrollService>();
        var controller = new PayrollController(serviceMock.Object);
        var dto = new CreatePayrollDto(1, 1m, 1m, 1m, 0m, 1);

        var result = await controller.AddPayrollAsync(dto);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task AddPayrollAsync_ServiceThrows_ReturnsBadRequest()
    {
        var serviceMock = new Mock<IPayrollService>();
        serviceMock.Setup(s => s.AddPayrollAsync(It.IsAny<CreatePayrollDto>()))
            .ThrowsAsync(new Exception("err"));
        var controller = new PayrollController(serviceMock.Object);

        var result = await controller.AddPayrollAsync(new CreatePayrollDto(1,1m,1m,1m,0m,1));

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdatePayrollAsync_ServiceThrows_ReturnsBadRequest()
    {
        var serviceMock = new Mock<IPayrollService>();
        serviceMock.Setup(s => s.UpdatePayrollAsync(It.IsAny<UpdatePayrollDto>()))
            .ThrowsAsync(new Exception("err"));
        var controller = new PayrollController(serviceMock.Object);

        var result = await controller.UpdatePayrollAsync(new UpdatePayrollDto(1,1m,1m,1m,0m,1));

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeletePayrollAsync_Success_ReturnsOk()
    {
        var serviceMock = new Mock<IPayrollService>();
        var controller = new PayrollController(serviceMock.Object);

        var result = await controller.DeletePayrollAsync(1);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeletePayrollAsync_ServiceThrows_ReturnsBadRequest()
    {
        var serviceMock = new Mock<IPayrollService>();
        serviceMock.Setup(s => s.DeletePayrollAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception("err"));
        var controller = new PayrollController(serviceMock.Object);

        var result = await controller.DeletePayrollAsync(1);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetPayrollByIdAsync_ReturnsNotFound()
    {
        var serviceMock = new Mock<IPayrollService>();
        serviceMock.Setup(s => s.GetPayrollByEmpIdAsync(It.IsAny<int>()))
            .ReturnsAsync((IEnumerable<PayrollDTO>?)null);
        var controller = new PayrollController(serviceMock.Object);

        var result = await controller.GetPayrollByIdAsync(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetPayrollsByDateAsync_ServiceThrows_ReturnsBadRequest()
    {
        var serviceMock = new Mock<IPayrollService>();
        serviceMock.Setup(s => s.GetEmpPayrollsByDateAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ThrowsAsync(new Exception("err"));
        var controller = new PayrollController(serviceMock.Object);

        var result = await controller.GetPayrollsByDateAsync(1, DateTime.Now, DateTime.Now);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}
