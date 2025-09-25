using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infractructure.Data.Configurations;

public class ActivityTypeConfigurations : IEntityTypeConfiguration<ActivityType>
{
    public void Configure(EntityTypeBuilder<ActivityType> builder)
    {
        builder.ToTable("ActivityType");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .HasMaxLength(Constants.NameMaxLength);

        builder.HasMany(at => at.Activities)
            .WithOne(a => a.ActivityType)
            .HasForeignKey(a => a.ActivityTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
