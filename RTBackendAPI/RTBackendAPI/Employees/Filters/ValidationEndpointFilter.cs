using FluentValidation;

namespace RTBackendAPI.Employees.Filters;

public sealed class ValidationEndpointFilter<T> : IEndpointFilter
{
    private readonly IValidator<T> _validator;

    public ValidationEndpointFilter(IValidator<T> validator)
    {
        this._validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argument = context.Arguments.OfType<T>().FirstOrDefault();

        if (argument == null)
        {
            return Results.BadRequest();
        }

        var validationResult = await this._validator.ValidateAsync(argument);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        return await next(context);
    }
}