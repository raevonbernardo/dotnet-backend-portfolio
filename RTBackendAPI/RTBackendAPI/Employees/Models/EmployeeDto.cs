using System.Text.Json.Serialization;

namespace RTBackendAPI.Employees.Models;

public sealed class EmployeeDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("first-name")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("last-name")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("job")]
    public string Job { get; set; } = string.Empty;

    [JsonPropertyName("employment-date")]
    public long EmploymentDateTimestamp { get; set; }

    public static EmployeeDto FromEmployee(Employee employee)
    {
        return new EmployeeDto
        {
            Id = employee.EmployeeId,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Job = employee.Job,
            EmploymentDateTimestamp = employee.EmploymentDateTimestamp,
        };
    }
}