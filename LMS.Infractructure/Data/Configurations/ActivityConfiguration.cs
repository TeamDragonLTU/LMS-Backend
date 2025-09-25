using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infractructure.Data.Configurations;

public class ActivityConfigurations : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable("Activity");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .HasMaxLength(Constants.NameMaxLength);

        builder.Property(a => a.Description)
            .HasMaxLength(Constants.DescriptionMaxLength);

        builder.HasOne(a => a.Module)
            .WithMany(m => m.Activities)
            .HasForeignKey(a => a.ModuleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => a.ModuleId)
            .HasDatabaseName("IX_Activity_ModuleId");

        builder.HasIndex(a => a.ActivityTypeId)
            .HasDatabaseName("IX_Activity_ActivityTypeId");
    }
}
