using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RTBackendAPI.Employees.Extensions;
using RTBackendAPI.Employees.Models;

namespace RTBackendAPI.Employees.Services;

public sealed class AuthenticationManager : IAuthenticationManager
{
    private readonly IConfigManager _configManager;
    
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public AuthenticationManager(IConfigManager configManager, JwtSecurityTokenHandler tokenHandler)
    {
        this._configManager = configManager;
        this._tokenHandler = tokenHandler;
    }

    public string CreateAuthToken(string username, AccessType accessType)
    {
        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.PreferredUsername, username),
            new Claim(ClaimTypes.Role, accessType.ToNonAllocString()),
        ];

        var jwtSettings = this._configManager.JwtSettings();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Token));
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(jwtSettings.ExpireSeconds),
            signingCredentials: creds);

        return this._tokenHandler.WriteToken(token);
    }
}