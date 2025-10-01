using LMS.Shared.DTOs.Module;

namespace LMS.Shared.DTOs.CourseDtos
{
    public record CourseDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTime StartDate { get; init; }
        public IEnumerable<ModuleDto>? Modules { get; init; }
    }
}
