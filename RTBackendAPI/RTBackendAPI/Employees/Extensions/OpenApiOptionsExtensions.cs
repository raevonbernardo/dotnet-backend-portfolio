using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using RTBackendAPI.Employees.Constants;

namespace RTBackendAPI.Employees.Extensions;

public static class OpenApiOptionsExtensions
{
    public static OpenApiOptions AddDefaultParameters(this OpenApiOptions options)
    {
        return options.AddOperationTransformer((operation, _, _) =>
        {
            operation.Parameters ??= new List<IOpenApiParameter>();
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = SharedConstants.API_HEADER_NAME,
                In = ParameterLocation.Header,
                Required = true,
            });

            return Task.CompletedTask;
        });
    }
}