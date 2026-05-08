using RTBackendAPI.Employees.Models;

namespace RTBackendAPI.Employees.Services;

public sealed class ConfigManager : IConfigManager
{
    private readonly IConfiguration _config;

    private readonly IPasswordManager _passwordManager;

    // needed a way to register any user if nothing exists in the database yet...
    private User? _defaultAdminUser { get; set; }

    public ConfigManager(IConfiguration config, IPasswordManager passwordManager)
    {
        this._config = config;
        this._passwordManager = passwordManager;
    }

    public JwtSettings JwtSettings()
    {
        return new JwtSettings(
            token: this._config["JwtSettings:Token"]!,
            issuer: this._config["JwtSettings:Issuer"]!,
            audience: this._config["JwtSettings:Audience"]!,
            expireSeconds: float.Parse(this._config["JwtSettings:ExpireSeconds"]!)
        );
    }

    public string ApiKey()
    {
        return this._config["Api-Key"]!;
    }

    public User DefaultAdminUser()
    {
        if (this._defaultAdminUser == null)
        {
            string username = this._config["DefaultAdminUser:Username"]!;
            string password = this._config["DefaultAdminUser:Password"]!;
            Guid publicId = new(this._config["DefaultAdminUser:PublicId"]!);
            string hashedPassword = this._passwordManager.HashPassword(username, password);
        
            this._defaultAdminUser = new User
            {
                PublicId = publicId,
                Username = username,
                HashedPassword = hashedPassword,
                AccessType = AccessType.Admin,
            };
        }
        
        return this._defaultAdminUser;
    }
}