using System.ComponentModel.DataAnnotations;

namespace Finlay.PharmaVigilance.Application.Validators.Attribute;

public class IdentityNumberValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string identityNumber || string.IsNullOrWhiteSpace(identityNumber))
            return new ValidationResult("Identity number is required.");

        if (identityNumber.Length != 11 || !identityNumber.All(char.IsDigit))
            return new ValidationResult("Identity number must be exactly 11 digits.");

        return ValidationResult.Success;
    }
}