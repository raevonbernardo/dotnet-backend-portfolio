using FluentValidation;
using RTBackendAPI.Employees.Commands;

namespace RTBackendAPI.Employees.Endpoints;

public static class AuthEndpoint
{
    public static void MapAuthEndpoint(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/auth");

        group.MapPost("/login", async (AuthenticateUserCommand command, AuthenticateUserCommandHandler handler, 
            IValidator<AuthenticateUserCommand> validator) =>
        {
            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            return await handler.Handle(command);
        }).AllowAnonymous();
    }
}