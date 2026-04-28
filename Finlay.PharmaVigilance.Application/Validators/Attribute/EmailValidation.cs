
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Finlay.PharmaVigilance.Application.Validators.Attribute;

public class EmailValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var email = value as string;

        if (string.IsNullOrWhiteSpace(email))
            return new ValidationResult("Email is required.");

        // 1. validación básica con MailAddress
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            if (addr.Address != email)
                return new ValidationResult("Invalid email format.");
        }
        catch
        {
            return new ValidationResult("Invalid email format.");
        }

        // 2. regex adicional (opcional pero más estricto)
        var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!regex.IsMatch(email))
            return new ValidationResult("Invalid email format.");

        return ValidationResult.Success;
    }
}