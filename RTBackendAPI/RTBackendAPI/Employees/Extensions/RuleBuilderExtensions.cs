using FluentValidation;

namespace RTBackendAPI.Employees.Extensions;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, string> IsAlphanumericOnly<T>(this IRuleBuilder<T, string> builder)
    {
        return builder.Matches(@"^[a-zA-Z0-9_]*$");
    }

    public static IRuleBuilderOptions<T, string> HasNoSpaces<T>(this IRuleBuilder<T, string> builder)
    {
        return builder.Must(value => !value.Contains(' '));
    }
}