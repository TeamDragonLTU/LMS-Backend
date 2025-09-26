using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations;

public class CourseConfigurations : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Course");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(50);

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        builder.HasMany(c => c.Users)
            .WithOne(u => u.Course)
            .HasForeignKey(u => u.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
