using RTBackendAPI.Employees.Services;

namespace RTBackendAPI.Employees.Commands;

public static class DeleteEmployeeExtensions
{
    public static IServiceCollection RegisterDeleteEmployeeDependencies(this IServiceCollection services)
    {
        return services.AddScoped<DeleteEmployeeCommandHandler>();
    }
}

public sealed class DeleteEmployeeCommandHandler
{
    private readonly IEmployeeDatabaseService _dbService;

    public DeleteEmployeeCommandHandler(IEmployeeDatabaseService dbService)
    {
        this._dbService = dbService;
    }

    public async Task<IResult> Handle(string employeeId)
    {
        var employee = await this._dbService.FindEmployeeByIdAsync(employeeId);

        if (employee == null)
        {
            return Results.NotFound("Employee doesn't exist.");
        }

        this._dbService.RemoveEmployee(employee);

        await this._dbService.SaveChangesAsync();

        return Results.NoContent();
    }
}