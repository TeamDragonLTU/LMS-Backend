using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infractructure.Data.Configurations;

public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("ApplicationUser");
        //Add more configurations here

        builder
            .HasOne(u => u.Course)
            .WithMany(c => c.Users)
            .HasForeignKey(u => u.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
