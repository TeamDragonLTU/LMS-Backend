namespace LMS.Shared.DTOs.CourseDtos
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }

        // Navigationproperty till Module här i framtiden
    }
}
