using RTBackendAPI.Employees.Models;

namespace RTBackendAPI.Employees.Services;

public interface IEmployeeDatabaseService
{
    Task<Employee?> FindEmployeeById(string employeeId);
}