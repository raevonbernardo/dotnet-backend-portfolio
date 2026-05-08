using RTBackendAPI.Employees.Constants;
using RTBackendAPI.Employees.Services;

namespace RTBackendAPI.Employees.Filters;

public static class ApiKeyValidationEndpointFilterExtensions
{
    public static RouteHandlerBuilder RequireApiKey(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<ApiKeyValidationEndpointFilter>();
    }
}

public sealed class ApiKeyValidationEndpointFilter : IEndpointFilter
{
    private readonly IConfigManager _configManager;

    public ApiKeyValidationEndpointFilter(IConfigManager configManager)
    {
        this._configManager = configManager;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(SharedConstants.API_HEADER_NAME, out var apiKeyValue))
        {
            return Results.Unauthorized();
        }

        string apiKey = this._configManager.ApiKey();

        if (!string.Equals(apiKeyValue, apiKey))
        {
            return Results.Unauthorized();
        }
        
        return await next(context);
    }
}