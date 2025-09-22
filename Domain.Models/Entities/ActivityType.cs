namespace Domain.Models.Entities
{
    public class ActivityType
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        // Navigation property
        public ICollection<Activity> Activities { get; set; } = new List<Activity>(); // 1:N - ActivityType:Activity

    }
}
