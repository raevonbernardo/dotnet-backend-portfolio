using FluentValidation;
using RTBackendAPI.Employees.Constants;
using RTBackendAPI.Employees.Extensions;
using RTBackendAPI.Employees.Models;
using RTBackendAPI.Employees.Services;

namespace RTBackendAPI.Employees.Commands;

public static class CreateEmployeeExtensions
{
    public static IServiceCollection RegisterCreateEmployeeDependencies(this IServiceCollection services)
    {
        return services
            .AddScoped<IValidator<CreateEmployeeCommand>, CreateEmployeeCommandValidator>()
            .AddScoped<CreateEmployeeCommandHandle>();
    } 
}

public sealed class CreateEmployeeCommand
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Job { get; set; } = string.Empty;
}

public sealed class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(command => command.FirstName)
            .NotNullOrEmpty()
            .WithMessage("First name required.")
            .ContainsLettersAndSpacesOnly()
            .WithMessage("First name contains invalid characters.")
            .MaximumLength(SharedConstants.EMPLOYEE_MAX_FIRST_NAME_LENGTH);
        
        RuleFor(command => command.LastName)
            .NotNullOrEmpty()
            .WithMessage("Last name required.")
            .ContainsLettersAndSpacesOnly()
            .WithMessage("Last name contains invalid characters.")
            .MaximumLength(SharedConstants.EMPLOYEE_MAX_LAST_NAME_LENGTH);

        RuleFor(command => command.Email)
            .NotNullOrEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email is invalid.")
            .MaximumLength(SharedConstants.EMPLOYEE_MAX_EMAIL_LENGTH);

        RuleFor(command => command.Job)
            .NotNullOrEmpty()
            .WithMessage("Job is required.")
            .ContainsLettersAndSpacesOnly()
            .WithMessage("Job contains invalid characters.")
            .MaximumLength(SharedConstants.EMPLOYEE_MAX_JOB_LENGTH);
    }
}

public sealed class CreateEmployeeCommandHandle
{
    private readonly IEmployeeDatabaseService _dbService;

    public CreateEmployeeCommandHandle(IEmployeeDatabaseService dbService)
    {
        this._dbService = dbService;
    }

    public async Task<IResult> Handle(CreateEmployeeCommand command)
    {
        AddEmployeeData data = AddEmployeeData.FromCreateEmployeeCommand(command);
        
        var newEmployee = await this._dbService.AddEmployeeAsync(data);

        await this._dbService.SaveChangesAsync();

        return Results.CreatedAtRoute(SharedConstants.ENDPOINTS_GET_EMPLOYEE_BY_ID, 
            new { id = newEmployee.EmployeeId });
    }
}