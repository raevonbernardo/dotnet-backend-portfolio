using System.Text.Json.Serialization;
using FluentValidation;
using RTBackendAPI.Employees.Extensions;
using RTBackendAPI.Employees.Services;

namespace RTBackendAPI.Employees.Commands;

public static class CreateUserExtensions
{
    public static IServiceCollection RegisterCreateUserDependencies(this IServiceCollection services)
    {
        return services
            .AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>()
            .AddScoped<CreateUserCommandHandler>();
    }
}

public sealed class CreateUserCommand
{
    [JsonPropertyName("username")] 
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("password")] 
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("api-key")] 
    public string ApiKey { get; set; } = string.Empty;
}

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(IConfigManager configManager)
    {
        RuleFor(command => command.Username)
            .ValidateUsername();

        RuleFor(command => command.Password)
            .ValidatePassword();

        RuleFor(command => command.ApiKey)
            .ValidateApiKey(configManager);
    }
}

public sealed class CreateUserCommandHandler
{
    private readonly IUserDatabaseService _dbService;

    public CreateUserCommandHandler(IUserDatabaseService dbService)
    {
        this._dbService = dbService;
    }

    public async Task<IResult> Handle(CreateUserCommand command)
    {
        if (IsDefaultAdmin(command.Username))
        {
            return Results.BadRequest("Username already taken.");
        }
        
        var user = await this._dbService.FindUserByUsernameAsync(command.Username);

        if (user != null)
        {
            return Results.BadRequest("User is already registered.");
        }

        await this._dbService.AddUser(command.Username, command.Password);

        await this._dbService.SaveChangesAsync();

        return Results.Created();
    }

    private bool IsDefaultAdmin(string username)
    {
        var defaultAdminUser = this._dbService.DefaultAdminUser();
        
        if (!string.Equals(username, defaultAdminUser.Username))
        {
            return false;
        }

        return true;
    }
}
