using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infractructure.Data.Configurations;

public class ModuleConfigurations : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.ToTable("Module");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .HasMaxLength(Constants.NameMaxLength);

        builder.Property(m => m.Description)
            .HasMaxLength(Constants.DescriptionMaxLength);

        builder.HasOne(m => m.Course)
            .WithMany(c => c.Modules)
            .HasForeignKey(m => m.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(m => m.CourseId)
            .HasDatabaseName("IX_Module_CourseId");

    }
}
