using RTBackendAPI.Employees.Commands;

namespace RTBackendAPI.Employees.Models;

public readonly struct AddEmployeeData
{
    public readonly string FirstName;

    public readonly string LastName;

    public readonly string Email;

    public readonly string Job;

    public AddEmployeeData(string firstName, string lastName, string email, string job)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Email = email;
        this.Job = job;
    }

    public static AddEmployeeData FromCreateEmployeeCommand(CreateEmployeeCommand command)
    {
        return new AddEmployeeData(command.FirstName, command.LastName, command.Email, command.Job);
    }
}