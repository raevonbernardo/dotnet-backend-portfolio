using Microsoft.EntityFrameworkCore;

namespace RTBackendAPI.Employees.Models;

public sealed class EmployeeDbContext : DbContext
{
    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();
}