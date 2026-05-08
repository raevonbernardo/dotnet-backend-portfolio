using System.Text.Json.Serialization;
using RTBackendAPI.Employees.Models;
using RTBackendAPI.Employees.Services;

namespace RTBackendAPI.Employees.Commands;

public static class UpdateUserAccessExtensions
{
    public static IServiceCollection RegisterUpdateUserAccessDependencies(this IServiceCollection services)
    {
        return services
            .AddScoped<UpdateUserAccessCommandHandler>();
    }
}

public sealed class UpdateUserAccessCommand
{
    [JsonPropertyName("access-type")]
    public AccessType AccessType { get; set; }
}

public sealed class UpdateUserAccessCommandHandler
{
    private readonly IUserDatabaseService _dbService;

    public UpdateUserAccessCommandHandler(IUserDatabaseService dbService)
    {
        this._dbService = dbService;
    }

    public async Task<IResult> Handle(Guid publicId, UpdateUserAccessCommand command)
    {
        var user = await this._dbService.FindUserByPublicIdAsync(publicId);

        if (user == null)
        {
            return Results.BadRequest("User does not exist.");
        }

        user.AccessType = command.AccessType;

        await this._dbService.SaveChangesAsync();

        return Results.NoContent();
    }
}