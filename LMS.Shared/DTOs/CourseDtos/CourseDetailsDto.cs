using LMS.Shared.DTOs.Module;

namespace LMS.Shared.DTOs.CourseDtos
{
    public class CourseDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }

        public ICollection<ModuleDto> Modules { get; set; } = new List<ModuleDto>();
    }
}
