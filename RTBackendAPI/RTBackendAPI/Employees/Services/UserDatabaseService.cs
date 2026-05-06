using Microsoft.EntityFrameworkCore;
using RTBackendAPI.Employees.Models;

namespace RTBackendAPI.Employees.Services;

public sealed class UserDatabaseService : IUserDatabaseService
{
    private readonly UserDbContext _dbContext;

    private readonly IConfigManager _configManager;

    public UserDatabaseService(UserDbContext dbContext, IConfigManager configManager)
    {
        this._dbContext = dbContext;
        this._configManager = configManager;
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
}