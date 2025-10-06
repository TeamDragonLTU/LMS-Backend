using LMS.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.DTOs
{
    public record ManipulateBaseDto
    {
        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(Constants.NameMaxLength, MinimumLength = Constants.TextMinLength,
         ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [NotOnlyWhitespace]
        public string Name { get; init; } = string.Empty;

        [Required(ErrorMessage = "Description is a required field.")]
        [StringLength(Constants.DescriptionMaxLength, MinimumLength = Constants.TextMinLength,
         ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [NotOnlyWhitespace]
        public string Description { get; init; } = string.Empty;
    }
}
