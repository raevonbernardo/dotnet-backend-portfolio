using FluentValidation;
using RTBackendAPI.Employees.Commands;
using RTBackendAPI.Employees.Extensions;

namespace RTBackendAPI.Employees.Endpoints;

public static class AuthEndpoint
{
    public static IEndpointRouteBuilder MapAuthEndpoint(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/auth");

        group.MapPost("/login", 
            async (AuthenticateUserCommand command, AuthenticateUserCommandHandler handler, 
            IValidator<AuthenticateUserCommand> validator) =>
        {
            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            return await handler.Handle(command);
        }).AllowAnonymous();

        group.MapPost("/register",
            async (CreateUserCommand command, IValidator<CreateUserCommand> validator,
                CreateUserCommandHandler handler) =>
            {
                var validationResult = await validator.ValidateAsync(command);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                return await handler.Handle(command);
            }).RequireAdminRoleAuthorization();

        group.MapPatch("update/{id:guid}",
            async (Guid id, UpdateUserAccessCommand command, UpdateUserAccessCommandHandler handler) =>
            {
                return await handler.Handle(id, command);
            }).RequireAdminRoleAuthorization();

        return builder;
    }
}