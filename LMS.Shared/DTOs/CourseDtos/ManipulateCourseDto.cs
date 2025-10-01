using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.DTOs.CourseDtos
{
    public record ManipulateCourseDto : ManipulateBaseDto
    {
        [Required(ErrorMessage = "Activity StartTime is a required field.")]
        public DateTime? StartDate { get; set; }
    }
}
