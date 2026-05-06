namespace RTBackendAPI.Employees.Models;

public readonly struct JwtSettings
{
    public readonly string Token;

    public readonly string Issuer;

    public readonly string Audience;

    public readonly float ExpireSeconds;

    public JwtSettings(string token, string issuer, string audience, float expireSeconds)
    {
        this.Token = token;
        this.Issuer = issuer;
        this.Audience = audience;
        this.ExpireSeconds = expireSeconds;
    }
}