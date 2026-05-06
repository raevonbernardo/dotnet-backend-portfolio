using Microsoft.AspNetCore.Identity;
using RTBackendAPI.Employees.Models;

namespace RTBackendAPI.Employees.Services;

public sealed class ConfigManager : IConfigManager
{
    private readonly IConfiguration _config;

    // needed a way to register any user if nothing exists in the database yet...
    private User? _defaultAdminUser { get; set; }

    public ConfigManager(IConfiguration config)
    {
        this._config = config;
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

    public bool TryGetApiKey(AccessType accessType, out string apiKey)
    {
        apiKey = string.Empty;
        
        switch (accessType)
        {
            case AccessType.Default:
                apiKey = this._config["Api-Keys:Default"]!;
                break;
            case AccessType.Admin:
                apiKey = this._config["Api-Keys:Admin"]!;
                break;
            default:
                return false;
        }

        return !string.IsNullOrWhiteSpace(apiKey);
    }

    public User DefaultAdminUser()
    {
        if (this._defaultAdminUser == null)
        {
            string username = this._config["DefaultAdminUser:Username"]!;
            string password = this._config["DefaultAdminUser:Password"]!;
            string hashedPassword = new PasswordHasher<string>().HashPassword(username, password);
        
            this._defaultAdminUser = new User
            {
                Username = username,
                HashedPassword = hashedPassword,
                AccessType = AccessType.Admin,
                IsActivated = true,
            };
        }
        
        return this._defaultAdminUser;
    }
}