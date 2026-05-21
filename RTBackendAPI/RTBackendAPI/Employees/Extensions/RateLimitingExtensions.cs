using RTBackendAPI.Employees.Constants;

namespace RTBackendAPI.Employees.Extensions;

public static class RateLimitingExtensions
{
    public static RouteHandlerBuilder RequireRateLimitingWithTokenPolicy(this RouteHandlerBuilder builder)
    {
        return builder.RequireRateLimiting(SharedConstants.RATE_LIMITER_POLICY_NAME);
    }
}