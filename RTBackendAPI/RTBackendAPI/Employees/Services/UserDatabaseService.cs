using Microsoft.EntityFrameworkCore;
using RTBackendAPI.Employees.Models;

namespace RTBackendAPI.Employees.Services;

public sealed class UserDatabaseService : IUserDatabaseService
{
    private readonly UserDbContext _dbContext;

    private readonly IConfigManager _configManager;

    private readonly IPasswordManager _passwordManager;

    public UserDatabaseService(UserDbContext dbContext, IConfigManager configManager, IPasswordManager passwordManager)
    {
        this._dbContext = dbContext;
        this._configManager = configManager;
        this._passwordManager = passwordManager;
    }

    public async Task<bool> HasAnyUser(AccessType accessType)
    {
        return await this._dbContext.Users.AnyAsync(user => user.AccessType == accessType);
    }

    public User DefaultAdminUser()
    {
        return this._configManager.DefaultAdminUser();
    }

    public async Task<User?> FindUserByUsernameAsync(string username)
    {
        return await this._dbContext.Users.FirstOrDefaultAsync(user => user.Username == username);
    }

    public async Task<User?> FindUserByPublicIdAsync(Guid publicId)
    {
        return await this._dbContext.Users.FirstOrDefaultAsync(user => user.PublicId == publicId);
    }

    public async Task AddUser(string username, string password)
    {
        User user = new()
        {
            Username = username,
            PublicId = Guid.NewGuid(),
            HashedPassword = this._passwordManager.HashPassword(username, password),
            AccessType = AccessType.Default,
        };

        await this._dbContext.Users.AddAsync(user);
    }
    
    public async Task<bool> RemoveUser(Guid userId)
    {
        var user = await FindUserByPublicIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        this._dbContext.Remove(user);

        return true;
    }

    public async Task SaveChangesAsync()
    {
        await this._dbContext.SaveChangesAsync();
    }
}