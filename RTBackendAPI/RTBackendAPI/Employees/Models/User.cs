using System.ComponentModel.DataAnnotations;

namespace RTBackendAPI.Employees.Models;

public enum AccessType
{
    Default = 0,
    Admin = 1,
}

public sealed class User
{
    [Key]
    public int PrivateId { get; set; }

    [MaxLength(20)]
    public required string Username { get; set; } = string.Empty;

    [MaxLength(100)]
    public required string HashedPassword { get; set; } = string.Empty;

    public AccessType AccessType { get; set; } = AccessType.Default;
}