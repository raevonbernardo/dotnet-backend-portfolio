using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RTBackendAPI.Employees.Commands;
using RTBackendAPI.Employees.Constants;
using RTBackendAPI.Employees.Extensions;
using RTBackendAPI.Employees.Filters;
using RTBackendAPI.Employees.Queries;

namespace RTBackendAPI.Employees.Endpoints;

public static class EmployeeEndpoint
{
    public static IEndpointRouteBuilder MapEmployeeEndpoint(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/employees");

        group.MapGet("/{id}", async (string id, [FromServices] IValidator<GetEmployeeByIdQuery> validator, 
                [FromServices] GetEmployeeByIdQueryHandler handler) =>
            {
                var query = new GetEmployeeByIdQuery { EmployeeId = id };
                
                var validationResult = await validator.ValidateAsync(query);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                return await handler.Handle(query);
            })
            .RequireApiKey()
            .RequireAuthorization()
            .WithName(SharedConstants.ENDPOINTS_GET_EMPLOYEE_BY_ID);

        group.MapPost("/register",
            async (CreateEmployeeCommand command, IValidator<CreateEmployeeCommand> validator,
                CreateEmployeeCommandHandler handler) =>
            {
                var validationResult = await validator.ValidateAsync(command);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                return await handler.Handle(command);
            })
            .RequireApiKey()
            .RequireAdminRoleAuthorization();

        group.MapPatch("/update/{id}",
            async (string id, [FromBody] UpdateEmployeeCommand command, [FromServices] IValidator<UpdateEmployeeCommand> validator,
                [FromServices] UpdateEmployeeCommandHandler handler) =>
            {
                var validationResult = await validator.ValidateAsync(command);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                return await handler.Handle(id, command);
            })
            .RequireApiKey()
            .RequireAdminRoleAuthorization();

        group.MapDelete("/unregister/{id}",
            async (string id, [FromServices] DeleteEmployeeCommandHandler handler) =>
            {
                return handler.Handle(id);
            })
            .RequireApiKey()
            .RequireAdminRoleAuthorization();

        return builder;
    }
}