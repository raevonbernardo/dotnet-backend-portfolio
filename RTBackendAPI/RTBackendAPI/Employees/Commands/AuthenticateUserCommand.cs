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

    private readonly IPasswordHasher<string> _passwordHasher;

    public AuthenticateUserCommandHandler(IUserDatabaseService dbService, IAuthenticationManager authManager, 
        IPasswordHasher<string> passwordHasher)
    {
        this._dbService = dbService;
        this._authManager = authManager;
        this._passwordHasher = passwordHasher;
    }

    public async Task<IResult> Handle(AuthenticateUserCommand command)
    {
        var user = await FindActiveUser(command);

        if (user == null)
        {
            return Results.Unauthorized();
        }

        string token = this._authManager.CreateAuthToken(user.Username, user.AccessType);

        return Results.Ok(token);
    }

    private async Task<User?> FindActiveUser(AuthenticateUserCommand command)
    {
        var user = await this._dbService.FindUserByUsernameAsync(command.Username);

        if (user == null || !user.IsActivated)
        {
            return null;
        }

        var verifyPasswordResult = this._passwordHasher.VerifyHashedPassword(user.Username, 
            user.HashedPassword, command.Password);

        switch (verifyPasswordResult)
        {
            case PasswordVerificationResult.SuccessRehashNeeded:
                // fall-through,
                // but can add some rehash logic in the future
            case PasswordVerificationResult.Success:
                return user;
            case PasswordVerificationResult.Failed:
                // fall-through
            default:
                return null;
        }
    }
}