using Microsoft.AspNetCore.Identity;

namespace RTBackendAPI.Employees.Services;

public sealed class PasswordManager : IPasswordManager
{
    private readonly IPasswordHasher<string> _passwordHasher;

    public PasswordManager(IPasswordHasher<string> passwordHasher)
    {
        this._passwordHasher = passwordHasher;
    }

    public string HashPassword(string user, string password)
    {
        return this._passwordHasher.HashPassword(user, password);
    }

    public bool IsPasswordValid(string user, string hashedPassword, string password)
    {
        var verifyPasswordResult = this._passwordHasher.VerifyHashedPassword(user, 
            hashedPassword, password);

        switch (verifyPasswordResult)
        {
            case PasswordVerificationResult.SuccessRehashNeeded:
            // fall-through
            case PasswordVerificationResult.Success:
                return true;
            case PasswordVerificationResult.Failed:
            // fall-through
            default:
                return false;
        }
    }
}