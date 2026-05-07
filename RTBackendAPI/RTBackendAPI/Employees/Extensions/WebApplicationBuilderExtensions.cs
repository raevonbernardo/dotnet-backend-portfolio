using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RTBackendAPI.Employees.Commands;
using RTBackendAPI.Employees.Models;
using RTBackendAPI.Employees.Services;

namespace RTBackendAPI.Employees.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder RegisterDependencies(this WebApplicationBuilder builder)
    {
        builder.Services
            .RegisterDatabases()
            .RegisterSingletons()
            .RegisterServices()
            .RegisterAuthenticateUserDependencies()
            .RegisterCreateUserDependencies();

        return builder;
    }

    private static IServiceCollection RegisterDatabases(this IServiceCollection services)
    {
        return services
            .AddDbContext<UserDbContext>(options =>
            {
                options.UseSqlite("Data Source=user.db");
            });
    }

    private static IServiceCollection RegisterSingletons(this IServiceCollection services)
    {
        return services
            .AddSingleton<IPasswordHasher<string>, PasswordHasher<string>>()
            .AddSingleton<JwtSecurityTokenHandler>()
            .AddSingleton<IConfigManager, ConfigManager>()
            .AddSingleton<IPasswordManager, PasswordManager>()
            .AddSingleton<IAuthenticationManager, AuthenticationManager>();
    }

    private static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IUserDatabaseService, UserDatabaseService>();
    }
}