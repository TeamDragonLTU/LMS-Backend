using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.Validation
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
            ErrorMessage = "{0} must be greater than {1}.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var currentValue = (DateTime)value!;
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException("Property not found");

            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance)!;

            if (currentValue <= comparisonValue)
            {
                string errorMessage;

                if (ErrorMessageString.Contains("{0}") && ErrorMessageString.Contains("{1}"))
                {
                    errorMessage = string.Format(
                        ErrorMessageString,
                        validationContext.DisplayName,
                        _comparisonProperty);
                }
                else if (ErrorMessageString.Contains("{0}"))
                {
                    errorMessage = string.Format(
                        ErrorMessageString,
                        validationContext.DisplayName);
                }
                else
                {
                    errorMessage = ErrorMessageString;
                }

                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
