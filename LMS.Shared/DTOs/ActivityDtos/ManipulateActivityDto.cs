using LMS.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.DTOs.ActivityDtos
{
    public record ManipulateActivityDto : ManipulateBaseDto
    {
        [Required(ErrorMessage = "Activity StartTime is a required field.")]
        public DateTime? StartTime { get; init; }

        [Required(ErrorMessage = "Activity EndTime is a required field.")]
        [DateGreaterThan(nameof(StartTime))]
        public DateTime? EndTime { get; init; }

        [RequiredGuid(ErrorMessage = "Activity ActivityTypeId is a required field.")]
        public Guid? ActivityTypeId { get; init; }

        [RequiredGuid(ErrorMessage = "Activity ModuleId is a required field.")]
        public Guid? ModuleId { get; init; }
    }
}
