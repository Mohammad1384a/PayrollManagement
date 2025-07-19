using AutoMapper;
using Task.Core.DTOs;
using Task.Infrastructure.Model;

namespace Task.API;

public class MappingProfile : Profile {
    public MappingProfile() {
        //payroll
        CreateMap<CreatePayrollDto, EmployeePayRoll>();
        CreateMap<UpdatePayrollDto, EmployeePayRoll>();
        CreateMap<EmployeePayRoll, PayrollDTO>();

        //employee
        CreateMap<CreateEmployeeDTO, Employee>();
        CreateMap<Employee, CreateEmployeeDTO>();
        CreateMap<CreatePayrollDto, EmployeePayRoll>();

        CreateMap<EmployeePayRoll, PayrollDTO>();
    }
}
