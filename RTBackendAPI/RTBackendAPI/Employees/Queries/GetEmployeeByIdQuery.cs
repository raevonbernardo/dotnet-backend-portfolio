using FluentValidation;
using RTBackendAPI.Employees.Constants;
using RTBackendAPI.Employees.Extensions;
using RTBackendAPI.Employees.Models;
using RTBackendAPI.Employees.Services;

namespace RTBackendAPI.Employees.Queries;

public static class GetEmployeeByIdExtensions
{
    public static IServiceCollection RegisterGetEmployeeByIdDependencies(this IServiceCollection services)
    {
        return services
            .AddScoped<IValidator<GetEmployeeByIdQuery>, GetEmployeeByIdQueryValidator>()
            .AddScoped<GetEmployeeByIdQueryHandler>();
    }
}

public sealed class GetEmployeeByIdQuery
{
    public string EmployeeId { get; set; } = string.Empty;
    
    // ef core framework looks for this method... magic.
    public static bool TryParse(string? value, out GetEmployeeByIdQuery? result)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            result = null;
            return false;
        }

        result = new GetEmployeeByIdQuery
        {
            EmployeeId = value,
        };
        
        return true;
    }
}

public sealed class GetEmployeeByIdQueryValidator : AbstractValidator<GetEmployeeByIdQuery>
{
    public GetEmployeeByIdQueryValidator()
    {
        RuleFor(query => query.EmployeeId)
            .NotNullOrEmpty()
            .MaximumLength(SharedConstants.EMPLOYEE_MAX_ID_LENGTH)
            .IsAlphanumericOnly()
            .WithMessage("Invalid Employee ID.");
    }
}

public sealed class GetEmployeeByIdQueryHandler
{
    private readonly IEmployeeDatabaseService _dbService;

    public GetEmployeeByIdQueryHandler(IEmployeeDatabaseService dbService)
    {
        this._dbService = dbService;
    }

    public async Task<IResult> Handle(GetEmployeeByIdQuery query)
    {
        var employee = await this._dbService.FindEmployeeById(query.EmployeeId);

        if (employee == null)
        {
            return Results.NotFound("Employee doesn't exist in database.");
        }

        var dto = EmployeeDto.FromEmployee(employee);

        return Results.Ok(dto);
    }
}