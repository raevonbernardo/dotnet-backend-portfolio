namespace RTBackendAPI.Employees.Services;

public interface IPasswordManager
{
    string HashPassword(string user, string password);

    bool IsPasswordValid(string user, string hashedPassword, string password);
}