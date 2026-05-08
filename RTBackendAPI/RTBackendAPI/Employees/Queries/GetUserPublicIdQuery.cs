using FluentValidation;
using RTBackendAPI.Employees.Extensions;
using RTBackendAPI.Employees.Services;

namespace RTBackendAPI.Employees.Queries;

public static class GetUserPublicIdExtensions
{
    public static IServiceCollection RegisterGetUserPublicIdDependencies(this IServiceCollection services)
    {
        return services
            .AddScoped<IValidator<GetUserPublicIdQuery>, GetUserPublicIdQueryValidator>()
            .AddScoped<GetUserPublicIdQueryHandler>();
    }
}

public sealed class GetUserPublicIdQuery
{
    public string Username { get; set; } = string.Empty;
}

public sealed class GetUserPublicIdQueryValidator : AbstractValidator<GetUserPublicIdQuery>
{
    public GetUserPublicIdQueryValidator()
    {
        RuleFor(query => query.Username)
            .NotNullOrEmpty()
            .IsAlphanumericOnly()
            .WithMessage("Invalid username.");
    }
}

public sealed class GetUserPublicIdQueryHandler
{
    private readonly IUserDatabaseService _dbService;

    public GetUserPublicIdQueryHandler(IUserDatabaseService dbService)
    {
        this._dbService = dbService;
    }

    public async Task<IResult> Handle(GetUserPublicIdQuery query)
    {
        var user = await this._dbService.FindUserByUsernameAsync(query.Username);

        if (user == null)
        {
            return Results.BadRequest("User doesn't exist.");
        }

        return Results.Ok(user.PublicId);
    }
}