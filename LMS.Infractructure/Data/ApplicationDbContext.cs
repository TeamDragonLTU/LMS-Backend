using Domain.Models.Entities;
using LMS.Infractructure.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LMS.Infractructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Module> Module { get; set; } 

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ApplicationUserConfigurations());
            // Help needed
            builder.Entity<Module>() 
                .HasOne(m => m.Course)
                .WithMany(k => k.Module) 
                .HasForeignKey(m => m.CourseID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
