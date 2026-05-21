using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RTBackendAPI.Employees.Commands;
using RTBackendAPI.Employees.Constants;
using RTBackendAPI.Employees.Models;
using RTBackendAPI.Employees.Queries;
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
            .RegisterGetUserPublicIdDependencies()
            .RegisterCreateUserDependencies()
            .RegisterUpdateUserAccessDependencies()
            .RegisterGetEmployeeByIdDependencies()
            .RegisterCreateEmployeeDependencies()
            .RegisterDeleteUserDependencies()
            .RegisterUpdateEmployeeDependencies()
            .RegisterDeleteEmployeeDependencies()
            .RegisterRateLimiter();

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

    private static IServiceCollection RegisterRateLimiter(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();

        var configManager = serviceProvider.GetRequiredService<IConfigManager>();
        TokenBucketRateLimiterSettings settings = configManager.RateLimiterSettings();
        
        services.AddRateLimiter(options =>
        {
            options.AddTokenBucketLimiter(policyName: SharedConstants.RATE_LIMITER_POLICY_NAME, configureOptions =>
            {
                configureOptions.TokenLimit = settings.TokenLimit;
                configureOptions.TokensPerPeriod = settings.TokenPerPeriod;
                configureOptions.ReplenishmentPeriod = settings.ReplenishmentPeriod;
                configureOptions.QueueLimit = settings.QueueLimit;
                configureOptions.AutoReplenishment = settings.AutoReplenishment;
            });

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        return services; 
    }
}