using RTBackendAPI.Employees.Models;

namespace RTBackendAPI.Employees.Services;

public interface IConfigManager
{
    JwtSettings JwtSettings();

    string ApiKey();
    
    User DefaultAdminUser();
    
    TokenBucketRateLimiterSettings RateLimiterSettings();
}