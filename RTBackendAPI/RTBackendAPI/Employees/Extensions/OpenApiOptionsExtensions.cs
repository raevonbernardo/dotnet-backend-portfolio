using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using RTBackendAPI.Employees.Constants;

namespace RTBackendAPI.Employees.Extensions;

public static class OpenApiOptionsExtensions
{
    public static OpenApiOptions AddDefaultParameters(this OpenApiOptions options)
    {
        // just searched and copied this from ai and tweaked + cleaned it up a bit...
        // still researching about open api documentation.
        return options
            .AddApiKeyHeaderDocumentation()
            .AddJwtBearerHeaderDocumentation();
    }

    private static OpenApiOptions AddApiKeyHeaderDocumentation(this OpenApiOptions options)
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

    private static OpenApiOptions AddJwtBearerHeaderDocumentation(this OpenApiOptions options)
    {
        return options
            .AddDocumentTransformer((document, _, _) =>
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your valid JWT token."
                };
                
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                document.Components.SecuritySchemes["Bearer"] = securityScheme;

                return Task.CompletedTask;
            })
            .AddOperationTransformer((operation, context, _) =>
            {
                var metadata = context.Description.ActionDescriptor.EndpointMetadata;
                var requiresAuthorization = metadata.Any(m => m is IAuthorizeData);
                var allowAnonymous = metadata.Any(m => m is IAllowAnonymous);

                if (requiresAuthorization && !allowAnonymous)
                {
                    operation.Security ??= new List<OpenApiSecurityRequirement>();
                    
                    operation.Security.Add(new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = []
                    });
                }

                return Task.CompletedTask;
            });
    }
}