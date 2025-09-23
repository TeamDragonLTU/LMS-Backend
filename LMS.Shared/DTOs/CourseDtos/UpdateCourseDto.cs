using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.DTOs.CourseDtos
{
    public class UpdateCourseDto
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
    }
}
