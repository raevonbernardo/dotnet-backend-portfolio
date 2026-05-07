using RTBackendAPI.Employees.Models;

namespace RTBackendAPI.Employees.Services;

public interface IUserDatabaseService
{
    Task<bool> HasAnyUser();
    
    User? DefaultAdminUser();
    
    Task<User?> FindUserByUsernameAsync(string username);

    Task<User?> FindUserByPublicIdAsync(Guid publicId);
    
    Task AddUser(string username, string password);
    
    Task SaveChangesAsync();
}