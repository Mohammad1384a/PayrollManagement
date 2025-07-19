using Microsoft.AspNetCore.Mvc;
using Task.Core.DTOs;
using Task.Core.Services.Interfaces;

namespace Task.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayrollController(IPayrollService payrollService) : ControllerBase{
    private readonly IPayrollService _payrollService = payrollService;

    /// <summary>
    /// Create a new payroll entry.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("Add")]
    public async Task<IActionResult> AddPayrollAsync([FromBody] CreatePayrollDto dto) {
        try {
            await _payrollService.AddPayrollAsync(dto);
            return Ok("Payroll added successfully.");
        }catch(Exception ex) {
            return BadRequest($"Error adding payroll: {ex.Message}");
        }
    }

    /// <summary>
    /// Update a specific payroll
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("Update")]
    public async Task<IActionResult> UpdatePayrollAsync([FromBody] UpdatePayrollDto dto) {
        try {
            await _payrollService.UpdatePayrollAsync(dto);
            return Ok("Payroll updated successfully.");
        } catch (Exception ex) {
            return BadRequest($"Error adding payroll: {ex.Message}");
        }
    }

    /// <summary>
    /// delete
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("Delete")]
    public async Task<IActionResult> DeletePayrollAsync([FromQuery]int id) {
        try {
            await _payrollService.DeletePayrollAsync(id);
            return Ok("Payroll deleted successfully.");
        } catch (Exception ex) {
            return BadRequest($"Error deleting payroll: {ex.Message}");
        }
    }

    /// <summary>
    /// get and employees payroll
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("getByEmpId")]
    public async Task<IActionResult> GetPayrollByIdAsync([FromQuery]int id) {
        try {
            var payroll = await _payrollService.GetPayrollByEmpIdAsync(id);
            if (payroll == null) {
                return NotFound("Payroll not found.");
            }
            return Ok(payroll);
        } catch (Exception ex) {
            return BadRequest($"Error retrieving payroll: {ex.Message}");
        }
    }

    /// <summary>
    /// get employees payrolls by date range
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    [HttpGet("getByDate")]
    public async Task<IActionResult> GetPayrollsByDateAsync(int employeeId,DateTime from, DateTime to) {
        try {
            var payrolls = await _payrollService.GetEmpPayrollsByDateAsync(employeeId,from, to);
            return Ok(payrolls);
        } catch (Exception ex) {
            return BadRequest($"Error retrieving payrolls: {ex.Message}");
        }
    }
}
