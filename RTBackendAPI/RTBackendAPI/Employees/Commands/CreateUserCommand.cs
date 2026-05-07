using System.Text.Json.Serialization;
using FluentValidation;
using RTBackendAPI.Employees.Constants;
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
            .NotNullOrEmpty()
            .WithMessage("{PropertName} cannot be empty.")
            .MinimumLength(SharedConstants.MIN_USERNAME_LENGTH)
            .WithMessage($"{{PropertName}} should contain at least {SharedConstants.MIN_USERNAME_LENGTH} characters.")
            .MaximumLength(SharedConstants.MAX_USERNAME_LENGTH)
            .WithMessage($"Max limit for {{PropertName}} is {SharedConstants.MAX_USERNAME_LENGTH}.")
            .IsAlphanumericOnly()
            .WithMessage("{PropertyName} should only contain alphanumeric characters.");
        
        RuleFor(command => command.Password)
            .Must(password => !string.IsNullOrWhiteSpace(password))
            .WithMessage("{PropertName} cannot be empty.")
            .MinimumLength(SharedConstants.MIN_PASSWORD_LENGTH)
            .WithMessage($"{{PropertName}} should contain at least {SharedConstants.MIN_PASSWORD_LENGTH} characters.")
            .MaximumLength(SharedConstants.MAX_PASSWORD_LENGTH)
            .WithMessage($"Max limit for {{PropertName}} is {SharedConstants.MAX_PASSWORD_LENGTH}.")
            .HasNoSpaces()
            .WithMessage("{PropertyName} should contain not contain spaces.");

        RuleFor(command => command.ApiKey)
            .NotNull()
            .NotEmpty()
            .Must(apiKey => string.Equals(apiKey, configManager.ApiKey()))
            .WithMessage("Invalid api key.");
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
        var user = await this._dbService.FindUserByUsernameAsync(command.Username);

        if (user != null)
        {
            return Results.BadRequest("User is already registered.");
        }

        await this._dbService.AddUser(command.Username, command.Password);

        await this._dbService.SaveChangesAsync();

        return Results.Created();
    }
}
