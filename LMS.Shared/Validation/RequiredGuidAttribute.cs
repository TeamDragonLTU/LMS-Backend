using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.Validation
{
    public class RequiredGuidAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                ErrorMessage = "{0} is required.";
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            if (value is Guid guid && guid == Guid.Empty)
            {
                ErrorMessage = "{0} cannot be empty.";
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}
