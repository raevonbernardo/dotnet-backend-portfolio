using FluentValidation;
using RTBackendAPI.Employees.Queries;

namespace RTBackendAPI.Employees.Endpoints;

public static class EmployeeEndpoint
{
    public static IEndpointRouteBuilder MapEmployeeEndpoint(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/employees");

        group.MapGet("/{query}", async (GetEmployeeByIdQuery query, IValidator<GetEmployeeByIdQuery> validator, 
                GetEmployeeByIdQueryHandler handler) =>
            {
                var validationResult = await validator.ValidateAsync(query);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                return await handler.Handle(query);
            })
            .WithName("GetEmployeeById");

        return builder;
    }
}