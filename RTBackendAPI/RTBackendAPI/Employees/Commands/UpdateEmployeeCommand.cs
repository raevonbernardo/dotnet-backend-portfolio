using FluentValidation;
using RTBackendAPI.Employees.Constants;
using RTBackendAPI.Employees.Extensions;
using RTBackendAPI.Employees.Services;

namespace RTBackendAPI.Employees.Commands;

public static class UpdateEmployeeExtensions
{
    public static IServiceCollection RegisterUpdateEmployeeDependencies(this IServiceCollection services)
    {
        return services
            .AddScoped<IValidator<UpdateEmployeeCommand>, UpdateEmployeeCommandValidator>()
            .AddScoped<UpdateEmployeeCommandHandler>();
    }
}

public sealed class UpdateEmployeeCommand
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Job { get; set; } = string.Empty;
}

public sealed class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(command => command.FirstName)
            .ContainsLettersAndSpacesOnly()
            .MaximumLength(SharedConstants.EMPLOYEE_MAX_FIRST_NAME_LENGTH)
            .WithMessage("First name is invalid.")
            .When(command => !string.IsNullOrWhiteSpace(command.FirstName));

        RuleFor(command => command.LastName)
            .ContainsLettersAndSpacesOnly()
            .MaximumLength(SharedConstants.EMPLOYEE_MAX_LAST_NAME_LENGTH)
            .WithMessage("Last name is invalid.")
            .When(command => !string.IsNullOrWhiteSpace(command.LastName));

        RuleFor(command => command.Email)
            .MaximumLength(SharedConstants.EMPLOYEE_MAX_EMAIL_LENGTH)
            .EmailAddress()
            .WithMessage("Email is invalid.")
            .When(command => !string.IsNullOrWhiteSpace(command.Email));

        RuleFor(command => command.Job)
            .ContainsLettersAndSpacesOnly()
            .MaximumLength(SharedConstants.EMPLOYEE_MAX_JOB_LENGTH)
            .WithMessage("Job is invalid.")
            .When(command => !string.IsNullOrWhiteSpace(command.Job));
    }
}

public sealed class UpdateEmployeeCommandHandler
{
    private readonly IEmployeeDatabaseService _dbService;

    public UpdateEmployeeCommandHandler(IEmployeeDatabaseService dbService)
    {
        this._dbService = dbService;
    }

    public async Task<IResult> Handle(string employeeId, UpdateEmployeeCommand command)
    {
        var employee = await this._dbService.FindEmployeeByIdAsync(employeeId);

        if (employee == null)
        {
            return Results.NotFound("Employee doesn't exist.");
        }

        if (!string.IsNullOrWhiteSpace(command.FirstName))
        {
            employee.FirstName = command.FirstName;
        }

        if (!string.IsNullOrWhiteSpace(command.LastName))
        {
            employee.LastName = command.LastName;
        }

        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            employee.Email = command.Email;
        }

        if (!string.IsNullOrWhiteSpace(command.Job))
        {
            employee.Job = command.Job;
        }

        await this._dbService.SaveChangesAsync();

        return Results.NoContent();
    }
}