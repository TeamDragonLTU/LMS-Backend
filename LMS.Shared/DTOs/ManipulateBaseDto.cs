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
        [RegularExpression(@"^[a-zA-Z0-9\s.\-/*"",:!?^]+$", ErrorMessage = "Name contains invalid characters. Only letters, numbers, spaces, and the following characters are allowed: . - / * , : ! ? ^ and double quotes")] // Against harmful strings, like html for XSS
        public string Name { get; init; } = string.Empty;

        [Required(ErrorMessage = "Description is a required field.")]
        [StringLength(Constants.NameMaxLength, MinimumLength = Constants.TextMinLength,
         ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [NotOnlyWhitespace]
        [RegularExpression(@"^[a-zA-Z0-9\s.\-/*"",:!?^]+$", ErrorMessage = "Description contains invalid characters. Only letters, numbers, spaces, and the following characters are allowed: . - / * , : ! ? ^  and double quotes")] // Against harmful strings, like html for XSS
        public string Description { get; init; } = string.Empty;
    }
}
