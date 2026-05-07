using System.ComponentModel.DataAnnotations;
using RTBackendAPI.Employees.Constants;

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
    
    public Guid PublicId { get; set; }
    
    [MaxLength(SharedConstants.MAX_USERNAME_LENGTH)]
    public required string Username { get; set; } = string.Empty;

    [MaxLength(SharedConstants.MAX_PASSWORD_HASHED_LENGTH)]
    public required string HashedPassword { get; set; } = string.Empty;

    public AccessType AccessType { get; set; } = AccessType.Default;
    
    public bool IsActivated { get; set; }
}