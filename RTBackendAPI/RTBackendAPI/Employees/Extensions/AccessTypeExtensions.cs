using RTBackendAPI.Employees.Models;

namespace RTBackendAPI.Employees.Extensions;

public static class AccessTypeExtensions
{
    public static string ToNonAllocString(this AccessType accessType)
    {
        switch (accessType)
        {
            case AccessType.Default:
                return nameof(AccessType.Default);
            case AccessType.Admin:
                return nameof(AccessType.Admin);
            default:
                return accessType.ToString();
        }
    }
}