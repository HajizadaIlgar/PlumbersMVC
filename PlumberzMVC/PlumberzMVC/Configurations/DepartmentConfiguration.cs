using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlumberzMVC.Models;

namespace PlumberzMVC.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder
            .HasKey(d => d.Id);
        builder
            .Property(d => d.DepartmentName)
            .IsRequired()
            .HasMaxLength(128);
    }
}
