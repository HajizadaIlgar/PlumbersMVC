using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlumberzMVC.Models;

namespace PlumberzMVC.Configurations;

public class WorkerConfiguration : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> builder)
    {
        builder
            .HasKey(w => w.Id);
        builder
            .Property(w => w.FullName)
            .IsRequired()
            .HasMaxLength(64);
        builder
            .Property(w => w.Designation)
            .HasMaxLength(32);
        builder
            .Property(w => w.ImageUrl)
            .IsRequired()
            .HasMaxLength(500);
        builder
            .HasOne(w => w.Department)
            .WithMany(w => w.Workers)
            .HasForeignKey(w => w.DepartmentId);
    }
}
