namespace RTBackendAPI.Employees.Constants;

public static class SharedConstants
{
    public const int MIN_USERNAME_LENGTH = 4;
    public const int MAX_USERNAME_LENGTH = 255;
    
    public const int MIN_PASSWORD_LENGTH = 8;
    public const int MAX_PASSWORD_LENGTH = 30;
    
    public const int MAX_PASSWORD_HASHED_LENGTH = 200;

    public const int EMPLOYEE_MAX_ID_LENGTH = 100;
    public const int EMPLOYEE_MAX_FIRST_NAME_LENGTH = 255;
    public const int EMPLOYEE_MAX_LAST_NAME_LENGTH = 255;
    public const int EMPLOYEE_MAX_EMAIL_LENGTH = 255;
    public const int EMPLOYEE_MAX_JOB_LENGTH = 255;
}
