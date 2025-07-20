using Microsoft.EntityFrameworkCore;
using Task.Core.Services;
using Task.Core.Services.Interfaces;
using Task.Infrastructure.Data;
using Task.Infrastructure.Repositories;
using Task.Infrastructure.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<IPayrollService, PayrollService>();
builder.Services.AddScoped<IEmployeePayrollRepository, EmployeePayrollRepository>();

builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy =>
        policy
            .WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod());
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
