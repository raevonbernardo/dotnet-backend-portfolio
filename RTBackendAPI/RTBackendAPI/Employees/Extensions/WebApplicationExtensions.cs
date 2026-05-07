using Microsoft.EntityFrameworkCore;
using RTBackendAPI.Employees.Models;

namespace RTBackendAPI.Employees.Extensions;

public static class WebApplicationExtensions
{
    public static void AutoMigrateDatabases(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var userDbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();

        userDbContext.Database.Migrate();

        var employeeDbContext = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();

        employeeDbContext.Database.Migrate();
    }
}