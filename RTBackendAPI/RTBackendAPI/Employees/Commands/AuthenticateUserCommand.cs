using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using RTBackendAPI.Employees.Constants;
using RTBackendAPI.Employees.Extensions;
using RTBackendAPI.Employees.Models;
using RTBackendAPI.Employees.Services;

namespace RTBackendAPI.Employees.Commands;

public sealed class AuthenticateUserCommand
{
    [JsonPropertyName("username")] 
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("password")] 
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("api-key")]
    public string ApiKey { get; set; } = string.Empty;
}

public sealed class AuthenticateUserCommandValidator : AbstractValidator<AuthenticateUserCommand>
{
    public AuthenticateUserCommandValidator(IConfigManager configManager)
    {
        RuleFor(command => command.Username)
            .NotNull()
            .NotEmpty()
            .MinimumLength(SharedConstants.MIN_USERNAME_LENGTH)
            .MaximumLength(SharedConstants.MAX_USERNAME_LENGTH)
            .IsAlphanumericOnly()
            .WithMessage("Invalid username.");

        RuleFor(command => command.Password)
            .NotNull()
            .NotEmpty()
            .MinimumLength(SharedConstants.MIN_PASSWORD_LENGTH)
            .MaximumLength(SharedConstants.MAX_PASSWORD_LENGTH)
            .HasNoSpaces()
            .WithMessage("Invalid password.");

        RuleFor(command => command.ApiKey)
            .NotNull()
            .NotEmpty()
            .Must(apiKey => string.Equals(apiKey, configManager.ApiKey()))
            .HasNoSpaces()
            .WithMessage("Invalid api key.");
    }
}

public sealed class AuthenticateUserCommandHandler
{
    private readonly IUserDatabaseService _dbService;

    private readonly IAuthenticationManager _authManager;

    private readonly IPasswordManager _passwordManager;

    public AuthenticateUserCommandHandler(IUserDatabaseService dbService, IAuthenticationManager authManager, 
        IPasswordManager passwordManager)
    {
        this._dbService = dbService;
        this._authManager = authManager;
        this._passwordManager = passwordManager;
    }

    public async Task<IResult> Handle(AuthenticateUserCommand command)
    {
        var user = await FindActiveUser(command);

        if (user == null)
        {
            return Results.Unauthorized();
        }

        string token = this._authManager.CreateAuthToken(user);

        return Results.Ok(token);
    }

    private async Task<User?> FindActiveUser(AuthenticateUserCommand command)
    {
        var user = await this._dbService.FindUserByUsernameAsync(command.Username);

        if (user == null || !user.IsActivated)
        {
            return null;
        }

        if (!this._passwordManager.IsPasswordValid(user.Username, user.HashedPassword, command.Password))
        {
            return null;
        }

        return user;
    }
}