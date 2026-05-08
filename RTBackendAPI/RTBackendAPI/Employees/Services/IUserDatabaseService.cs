using RTBackendAPI.Employees.Models;

namespace RTBackendAPI.Employees.Services;

public interface IUserDatabaseService
{
    Task<bool> HasAnyUser(AccessType accessType);
    
    User DefaultAdminUser();
    
    Task<User?> FindUserByUsernameAsync(string username);

    Task<User?> FindUserByPublicIdAsync(Guid publicId);
    
    Task AddUser(string username, string password);

    void RemoveUser(User user);
    
    Task SaveChangesAsync();
}