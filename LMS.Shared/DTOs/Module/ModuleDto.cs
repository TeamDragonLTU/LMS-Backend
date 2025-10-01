using LMS.Shared.DTOs.ActivityDtos;
using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.DTOs.Module
{
    public record ModuleDto
    {
        public Guid Id { get; init; }

        [Required]
        [StringLength(100)]
        public string Name { get; init; } = string.Empty;

        [StringLength(500)]
        public string Description { get; init; } = string.Empty;

        [Required]
        public DateTime StartDate { get; init; }

        [Required]
        public DateTime EndDate { get; init; }

        public IEnumerable<ActivityDto>? Activities { get; init; }

        //public IEnumerable<DocumentDto> Documents { get; init; }
    }
}
