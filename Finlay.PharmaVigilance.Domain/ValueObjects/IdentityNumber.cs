namespace Finlay.PharmaVigilance.Domain.ValueObjects;

/// <summary>
/// Represents a Cuban identity number (11 digits).
/// Encapsulates format validation, date of birth extraction, and age calculation.
/// </summary>
public class IdentityNumber : IEquatable<IdentityNumber>
{
    public string Value { get; }
    public DateTime DateOfBirth { get; }
    public int Age => CalculateAge(DateOfBirth);

    public IdentityNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Identity number cannot be empty.", nameof(value));

        if (value.Length != 11 || !value.All(char.IsDigit))
            throw new ArgumentException("Identity number must be exactly 11 digits.", nameof(value));

        Value = value;
        DateOfBirth = ExtractDateOfBirth(value);
    }

    private static DateTime ExtractDateOfBirth(string identityNumber)
    {
        string yy = identityNumber.Substring(0, 2);
        string mm = identityNumber.Substring(2, 2);
        string dd = identityNumber.Substring(4, 2);

        int year = int.Parse(yy);
        int month = int.Parse(mm);
        int day = int.Parse(dd);

        int currentYearTwoDigits = DateTime.Now.Year % 100;
        int fullYear = (year > currentYearTwoDigits) ? 1900 + year : 2000 + year;

        try
        {
            return new DateTime(fullYear, month, day);
        }
        catch
        {
            throw new ArgumentException("Invalid date encoded in identity number.");
        }
    }

    private static int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        int age = today.Year - dateOfBirth.Year;

        if (dateOfBirth.Date > today.AddYears(-age))
            age--;

        return age;
    }

    // Igualdad por valor: dos IdentityNumber son iguales si tienen el mismo Value
    public override bool Equals(object? obj)
        => Equals(obj as IdentityNumber);

    public bool Equals(IdentityNumber? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override int GetHashCode()
        => Value.GetHashCode();

    public static bool operator ==(IdentityNumber? left, IdentityNumber? right)
        => Equals(left, right);

    public static bool operator !=(IdentityNumber? left, IdentityNumber? right)
        => !Equals(left, right);

    public override string ToString() => Value;
}