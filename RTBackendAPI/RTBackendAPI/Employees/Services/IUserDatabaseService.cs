using RTBackendAPI.Employees.Models;

namespace RTBackendAPI.Employees.Services;

public interface IUserDatabaseService
{
    Task<User?> FindUserByUsernameAsync(string username);
    
    Task AddUser(string username, string password);
    
    Task SaveChangesAsync();
}