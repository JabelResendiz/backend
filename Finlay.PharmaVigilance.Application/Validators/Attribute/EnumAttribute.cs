using System.ComponentModel.DataAnnotations;

namespace Finlay.PharmaVigilance.Application.Validators.Attribute;


/*
Usarla despues en un Dto de esta forma:

[EnumAttribute(typeof(ReporterRelationship), ErrorMessage = "Invalid Reporter Relationship.")]
public string ReporterRelationship { get; set; } = string.Empty;

*/

public class EnumValidationAttribute : ValidationAttribute
{
    private readonly Type _enumType;

    public EnumValidationAttribute(Type enumType)
    {
        if (!enumType.IsEnum)
            throw new ArgumentException("Type must be an enum.");

        _enumType = enumType;
    }

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Value is required.");
        }

        var valueString = value.ToString();

        if (string.IsNullOrWhiteSpace(valueString))
        {
            return new ValidationResult("Value is required.");
        }

        bool isValid = System.Enum.TryParse(
            _enumType,
            valueString,
            true,
            out var parsedEnum
        );

        if (!isValid || parsedEnum == null || !System.Enum.IsDefined(_enumType, parsedEnum))
        {
            return new ValidationResult(
                $"'{valueString}' is not a valid value for {_enumType.Name}."
            );
        }

        return ValidationResult.Success;
    }
}