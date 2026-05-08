using Microsoft.AspNetCore.Authorization;
using RTBackendAPI.Employees.Models;

namespace RTBackendAPI.Employees.Extensions;

public static class AuthorizationExtensions
{
    public static RouteHandlerBuilder RequireAdminRoleAuthorization(this RouteHandlerBuilder builder)
    {
        return builder.RequireAuthorization(new AuthorizeAttribute { Roles = AccessType.Admin.ToNonAllocString() });
    }
}