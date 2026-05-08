using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            .RegisterJwtAuthentication()
            .RegisterAuthenticateUserDependencies()
            .RegisterCreateUserDependencies()
            .RegisterUpdateUserAccessDependencies();

        return builder;
    }

    private static IServiceCollection RegisterDatabases(this IServiceCollection services)
    {
        return services
            .AddDbContext<UserDbContext>(options =>
            {
                options.UseSqlite("Data Source=user.db");
            })
            .AddDbContext<EmployeeDbContext>(options =>
            {
                options.UseSqlite("Data Source=employee.db");
            });
    }
    
    private static IServiceCollection RegisterJwtAuthentication(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();

        var configManager = serviceProvider.GetRequiredService<IConfigManager>();
        JwtSettings jwtSettings = configManager.JwtSettings();
        
        services.AddAuthorization()
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Token)),
                    RequireExpirationTime = true,
                };
            });

        return services;
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
            .AddScoped<IUserDatabaseService, UserDatabaseService>()
            .AddScoped<IEmployeeDatabaseService, EmployeeDatabaseService>();
    }
}