using Domain.Models.Entities;
using LMS.Shared;
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
            .HasMaxLength(Constants.NameMaxLength);

        builder.Property(c => c.Description)
            .HasMaxLength(Constants.DescriptionMaxLength);

        builder.HasMany(c => c.Users)
            .WithOne(u => u.Course)
            .HasForeignKey(u => u.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
