namespace Task.Core.DTOs;
public record PayrollDTO(
    int Id,
    decimal BasicSalary,
    decimal Allowance,
    decimal Transportation,
    decimal TotalSalary,
    byte OverTimeCalc,
    DateTime Date
);

public record CreatePayrollDto(
    int EmployeeId,
    decimal BasicSalary,
    decimal Allowance,
    decimal Transportation,
    decimal TotalSalary,
    byte OverTimeCalc
);

public record UpdatePayrollDto(
    int Id,
    decimal BasicSalary,
    decimal Allowance,
    decimal Transportation,
    decimal TotalSalary,
    byte OverTimeCalc
);