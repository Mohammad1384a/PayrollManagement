using Task.Core.DTOs;
using Task.Core.Services.Interfaces;
using Task.Infrastructure.Model;
using Task.Infrastructure.Repositories.Interfaces;
using AutoMapper;
using Task.OvertimeCalculator;
using TaskType = System.Threading.Tasks.Task;

namespace Task.Core.Services;

public class PayrollService(IEmployeePayrollRepository repository, IMapper mapper, IEmployeeRepository empRepo) : IPayrollService {
    private readonly IEmployeePayrollRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IEmployeeRepository _empRepo = empRepo;
    private readonly IOverTimeCalc _overtimeCalc = new OverTimeCalc();


    private bool CheckOverTimeCalc(byte calc) {
        return calc < 4 && calc > 0;
    }
    public async TaskType AddPayrollAsync(CreatePayrollDto dto) {
        var emp = await _empRepo.GetByIdAsync(dto.EmployeeId);
        if (emp == null || !CheckOverTimeCalc(dto.OverTimeCalc)) {
            throw new Exception("invalid parameter");
        }
        var payroll = _mapper.Map<EmployeePayRoll>(dto);
        payroll.Date = DateTime.Now;
        payroll.TotalSalary = dto.BasicSalary + dto.Allowance + dto.Transportation + _overtimeCalc.CalcBasedOnType(dto.OverTimeCalc,dto.Allowance,dto.BasicSalary);
        await _repository.AddAsync(payroll);
    }

    public async TaskType UpdatePayrollAsync(UpdatePayrollDto dto) {
        var payroll = await _repository.GetByIdAsync(dto.PayrollId);
        if (payroll == null || !CheckOverTimeCalc(dto.OverTimeCalc)) throw new Exception("invalid parameter");

        _mapper.Map(dto, payroll);
        //todo - overtime
        payroll.TotalSalary = dto.BasicSalary + dto.Allowance + dto.Transportation + _overtimeCalc.CalcBasedOnType(dto.OverTimeCalc, dto.Allowance, dto.BasicSalary);
        payroll.Date = DateTime.Now;
        await _repository.UpdateAsync(payroll);
    }

    public async TaskType DeletePayrollAsync(int id){
        var payroll = await _repository.GetByIdAsync(id);
        if (payroll == null) {
            throw new Exception("not found payroll");
        }
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<PayrollDTO>?> GetPayrollByEmpIdAsync(int empId) {
        var emp = await _empRepo.GetByIdAsync(empId);
        if (emp == null) { 
            throw new Exception("not found emp");
        }
        var payroll = await _repository.GetByEmpIdAsync(empId);
        return _mapper.Map<IEnumerable<PayrollDTO>>(payroll);
    }

    public async Task<IEnumerable<PayrollDTO>> GetEmpPayrollsByDateAsync(int empId,DateTime from, DateTime to) {
        var emp = await _empRepo.GetByIdAsync(empId);
        if (emp == null) {
            throw new Exception("not found emp");
        }
        var payrolls = await _repository.GetByDateRangeForEmpAsync(empId,from, to);
        return _mapper.Map<IEnumerable<PayrollDTO>>(payrolls);
    }
}
