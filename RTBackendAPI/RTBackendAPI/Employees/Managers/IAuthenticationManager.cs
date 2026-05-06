using RTBackendAPI.Employees.Models;

namespace RTBackendAPI.Employees.Services;

public interface IAuthenticationManager
{
    string CreateAuthToken(string username, AccessType accessType);
}