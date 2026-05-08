using FluentValidation;
using RTBackendAPI.Employees.Extensions;
using RTBackendAPI.Employees.Services;

namespace RTBackendAPI.Employees.Commands;

public static class DeleteUserExtensions
{
    public static IServiceCollection RegisterDeleteUserDependencies(this IServiceCollection services)
    {
        return services
            .AddScoped<IValidator<DeleteUserCommand>, DeleteUserCommandValidator>()
            .AddScoped<DeleteUserCommandHandler>();
    }
}

public sealed class DeleteUserCommand
{
    public Guid UserId { get; set; }

    public string ApiKey { get; set; } = string.Empty;
}

public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator(IUserDatabaseService dbService, IConfigManager configManager)
    {
        RuleFor(command => command.UserId)
            .Must(userId => userId != dbService.DefaultAdminUser().PublicId)
            .WithMessage("Invalid user id.");

        RuleFor(command => command.ApiKey)
            .ValidateApiKey(configManager);
    }
}

public sealed class DeleteUserCommandHandler
{
    private readonly IUserDatabaseService _dbService;

    public DeleteUserCommandHandler(IUserDatabaseService dbService)
    {
        this._dbService = dbService;
    }

    public async Task<IResult> Handle(DeleteUserCommand command)
    {
        var user = await this._dbService.FindUserByPublicIdAsync(command.UserId);

        if (user == null)
        {
            return Results.NotFound("User not found.");
        }

        this._dbService.RemoveUser(user);

        await this._dbService.SaveChangesAsync();

        return Results.NoContent();
    }
}