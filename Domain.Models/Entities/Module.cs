namespace Domain.Models.Entities
{
    public class Module
    {

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Foreign key
        public Guid CourseId { get; set; }

        // Navigation properties
        public Course Course { get; set; } = null!;
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

        //public ICollection<Document> Documents { get; set; } = new List<Document>();

    }
}
