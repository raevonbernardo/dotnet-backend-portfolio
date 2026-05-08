using RTBackendAPI.Employees.Models;

namespace RTBackendAPI.Employees.Services;

public interface IEmployeeDatabaseService
{
    Task<Employee?> FindEmployeeByIdAsync(string employeeId);

    Task<Employee> AddEmployeeAsync(AddEmployeeData data);

    Task SaveChangesAsync();
}