using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.Validation
{
    public class NotOnlyWhitespaceAttribute : ValidationAttribute
    {
        public NotOnlyWhitespaceAttribute()
        {
            // Default error message
            ErrorMessage = "{0} cannot be only whitespace.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string str && !string.IsNullOrWhiteSpace(str))
                return ValidationResult.Success;

            // FormatErrorMessage automatically replaces {0}, if it is there in the error message, with the property display name 
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}
