using System.ComponentModel.DataAnnotations;
using RTBackendAPI.Employees.Constants;

namespace RTBackendAPI.Employees.Models;

public sealed class Employee
{
    [Key] 
    public int PrivateId { get; set; }

    [MaxLength(SharedConstants.EMPLOYEE_MAX_ID_LENGTH)]
    public string EmployeeId { get; set; } = string.Empty;

    [MaxLength(SharedConstants.EMPLOYEE_MAX_FIRST_NAME_LENGTH)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(SharedConstants.EMPLOYEE_MAX_LAST_NAME_LENGTH)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(SharedConstants.EMPLOYEE_MAX_EMAIL_LENGTH)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(SharedConstants.EMPLOYEE_MAX_JOB_LENGTH)]
    public string Job { get; set; } = string.Empty;

    public long EmploymentDateTimestamp { get; set; }
}