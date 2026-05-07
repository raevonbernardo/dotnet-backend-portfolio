using Microsoft.EntityFrameworkCore;

namespace RTBackendAPI.Employees.Models;

public sealed class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}