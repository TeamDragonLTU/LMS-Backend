using LMS.Shared.Validation;

namespace LMS.Shared.DTOs.Module
{
    public record CreateModuleDto : ManipulateModuleDto
    {
        [RequiredGuid(ErrorMessage = "Module CourseId is a required field.")]
        public Guid CourseId { get; init; }
    }
}
