using Microsoft.EntityFrameworkCore;
using RTBackendAPI.Employees.Models;

namespace RTBackendAPI.Employees.Services;

public sealed class EmployeeDatabaseService : IEmployeeDatabaseService
{
    private readonly EmployeeDbContext _dbContext;

    public EmployeeDatabaseService(EmployeeDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public Task<Employee?> FindEmployeeByIdAsync(string employeeId)
    {
        return this._dbContext.Employees.FirstOrDefaultAsync(employee =>
            string.Equals(employeeId, employee.EmployeeId));
    }

    public async Task<Employee> AddEmployeeAsync(AddEmployeeData data)
    {
        var employee = new Employee
        {
            EmployeeId = Guid.NewGuid().ToString(),
            FirstName = data.FirstName,
            LastName = data.LastName,
            Email = data.Email,
            Job = data.Job,
            EmploymentDateTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };

        await this._dbContext.Employees.AddAsync(employee);

        return employee;
    }

    public async Task SaveChangesAsync()
    {
        await this._dbContext.SaveChangesAsync();
    }
}