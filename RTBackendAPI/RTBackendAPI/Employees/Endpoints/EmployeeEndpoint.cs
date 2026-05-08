using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RTBackendAPI.Employees.Constants;
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
            }).RequireAuthorization()
            .WithName(SharedConstants.ENDPOINTS_GET_EMPLOYEE_BY_ID);

        return builder;
    }
}