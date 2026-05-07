using Microsoft.AspNetCore.Identity;
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

    public async Task<User?> FindUserByUsernameAsync(string username)
    {
        bool hasAny = await this._dbContext.Users.AnyAsync();

        if (!hasAny)
        {
            return this._configManager.DefaultAdminUser();
        }

        return await this._dbContext.Users.FirstOrDefaultAsync(user => user.Username == username);
    }

    public async Task AddUser(string username, string password)
    {
        User user = new()
        {
            Username = username,
            HashedPassword = this._passwordManager.HashPassword(username, password),
            AccessType = AccessType.Default,
            IsActivated = true,
        };

        await this._dbContext.Users.AddAsync(user);
    }

    public async Task SaveChangesAsync()
    {
        await this._dbContext.SaveChangesAsync();
    }
}