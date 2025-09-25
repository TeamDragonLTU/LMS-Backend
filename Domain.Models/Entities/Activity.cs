namespace Domain.Models.Entities
{
    public class Activity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; } = null!;

        public Guid ModuleId { get; set; } // Foreign key
        public Guid ActivityTypeId { get; set; } // Foreign key


        // Navigation properties
        public Module Module { get; set; } = null!;
        public ActivityType ActivityType { get; set; } = null!;

    }
}
