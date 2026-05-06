using RTBackendAPI.Employees.Models;

namespace RTBackendAPI.Employees.Services;

public interface IConfigManager
{
    JwtSettings JwtSettings();
    
    bool TryGetApiKey(AccessType accessType, out string apiKey);

    User DefaultAdminUser();
}