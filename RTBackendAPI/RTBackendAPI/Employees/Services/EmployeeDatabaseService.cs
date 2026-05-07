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

    public Task<Employee?> FindEmployeeById(string employeeId)
    {
        return this._dbContext.Employees.FirstOrDefaultAsync(employee =>
            string.Equals(employeeId, employee.EmployeeId));
    }
}