using LMS.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.DTOs.Module
{
    public record ManipulateModuleDto : ManipulateBaseDto
    {
        [Required(ErrorMessage = "Module StartDate is a required field.")]
        public DateTime? StartDate { get; init; }

        [Required(ErrorMessage = "Module EndDate is a required field.")]
        [DateGreaterThan(nameof(StartDate))]
        public DateTime? EndDate { get; init; }
    }
}
