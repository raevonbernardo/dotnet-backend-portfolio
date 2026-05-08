using FluentValidation;
using RTBackendAPI.Employees.Constants;
using RTBackendAPI.Employees.Services;

namespace RTBackendAPI.Employees.Extensions;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, string> NotNullOrEmpty<T>(this IRuleBuilder<T, string> builder)
    {
        return builder.Must(value => !string.IsNullOrWhiteSpace(value));
    }
    
    public static IRuleBuilderOptions<T, string> IsAlphanumericOnly<T>(this IRuleBuilder<T, string> builder)
    {
        return builder.Matches(@"^[a-zA-Z0-9_]*$");
    }

    public static IRuleBuilderOptions<T, string> HasNoSpaces<T>(this IRuleBuilder<T, string> builder)
    {
        return builder.Must(value => !value.Contains(' '));
    }

    public static IRuleBuilderOptions<T, string> ValidateUsername<T>(this IRuleBuilder<T, string> builder)
    {
        return builder
            .NotNullOrEmpty()
            .WithMessage("{PropertName} cannot be empty.")
            .MinimumLength(SharedConstants.MIN_USERNAME_LENGTH)
            .WithMessage($"{{PropertName}} should contain at least {SharedConstants.MIN_USERNAME_LENGTH} characters.")
            .MaximumLength(SharedConstants.MAX_USERNAME_LENGTH)
            .WithMessage($"Max limit for {{PropertName}} is {SharedConstants.MAX_USERNAME_LENGTH}.")
            .IsAlphanumericOnly()
            .WithMessage("{PropertyName} should only contain alphanumeric characters.");
    }
    
    public static IRuleBuilderOptions<T, string> ValidatePassword<T>(this IRuleBuilder<T, string> builder)
    {
        return builder
            .NotNullOrEmpty()
            .WithMessage("{PropertName} cannot be empty.")
            .MinimumLength(SharedConstants.MIN_PASSWORD_LENGTH)
            .WithMessage($"{{PropertName}} should contain at least {SharedConstants.MIN_PASSWORD_LENGTH} characters.")
            .MaximumLength(SharedConstants.MAX_PASSWORD_LENGTH)
            .WithMessage($"Max limit for {{PropertName}} is {SharedConstants.MAX_PASSWORD_LENGTH}.")
            .HasNoSpaces()
            .WithMessage("{PropertyName} should contain not contain spaces.");
    }
    
    public static IRuleBuilderOptions<T, string> ValidateApiKey<T>(this IRuleBuilder<T, string> builder, 
        IConfigManager configManager)
    {
        return builder
            .NotNullOrEmpty()
            .Must(apiKey => string.Equals(apiKey, configManager.ApiKey()))
            .WithMessage("Invalid api key.");
    }

    public static IRuleBuilderOptions<T, string> ContainsLettersAndSpacesOnly<T>(this IRuleBuilder<T, string> builder)
    {
        return builder.Must(name => name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)));
    }
}