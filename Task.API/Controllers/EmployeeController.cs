using Microsoft.AspNetCore.Mvc;
using Task.Core.DTOs;
using Task.Core.Services.Interfaces;

namespace Task.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController(IEmployeeService employeeService) : ControllerBase{
    private readonly IEmployeeService _employeeService = employeeService;


    [HttpPost("AddEmployee")]
    public async Task<IActionResult> AddEmployee(CreateEmployeeDTO dto) {
        try {
            await _employeeService.AddEmployeeAsync(dto);
            return Ok("Employee Created Successfully");
        }catch(Exception ex) {
            return BadRequest($"Error adding employee: {ex.Message}");
        }
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAsync() {
        try {
            return Ok(await _employeeService.GetAllAsync());
        } catch (Exception ex) {
            return BadRequest($"{ex.Message}");
        }
    }
}
