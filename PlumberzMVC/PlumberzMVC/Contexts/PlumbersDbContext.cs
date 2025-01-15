using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlumberzMVC.Models;

namespace PlumberzMVC.Contexts;

public class PlumbersDbContext : IdentityDbContext<AppUser>
{
    public PlumbersDbContext(DbContextOptions opt) : base(opt) { }

    public DbSet<Worker> Workers { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PlumbersDbContext).Assembly);
    }
}
