using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.DTOs.CourseDtos
{
    public class CreateCourseDto
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }

        // Navigationproperty till Module här i framtiden
    }
}
