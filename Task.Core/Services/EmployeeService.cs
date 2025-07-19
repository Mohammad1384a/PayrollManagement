using AutoMapper;
using Task.Core.DTOs;
using Task.Core.Services.Interfaces;
using Task.Infrastructure.Model;
using Task.Infrastructure.Repositories.Interfaces;
using TaskType = System.Threading.Tasks.Task;

namespace Task.Core.Services;
public class EmployeeService (IMapper mapper,IEmployeeRepository repository) : IEmployeeService{
    private readonly IEmployeeRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async TaskType AddEmployeeAsync(CreateEmployeeDTO dto) {
        var employee = _mapper.Map<Employee>(dto);
        await _repository.AddAsync(employee);
    }

    public async Task<IEnumerable<Employee>> GetAllAsync() {
        return await _repository.GetAllAsync();
    }
}


