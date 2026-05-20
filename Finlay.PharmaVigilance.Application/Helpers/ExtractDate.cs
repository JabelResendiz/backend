namespace Finlay.PharmaVigilance.Application.Helpers;

public static class ExtractDateHelper
{

    public static DateTime ExtractDateOfBirht(string identityNumber)
    {
        // Basic validation: Identity number should have a reasonable length
        if (identityNumber.Length != 11 || !identityNumber.All(char.IsDigit))
            throw new ArgumentException(
                "Identity number must be 11 digits",
                nameof(identityNumber));

        string yy = identityNumber.Substring(0, 2);
        string mm = identityNumber.Substring(2, 2);
        string dd = identityNumber.Substring(4, 2);

        int year = int.Parse(yy);
        int month = int.Parse(mm);
        int day = int.Parse(dd);

        int currentYearTwoDigits = DateTime.Now.Year % 100;
        int fullYear = (year > currentYearTwoDigits) ? 1900 + year : 2000 + year;

        DateTime extractedDate;

        try
        {
            extractedDate = new DateTime(fullYear, month, day);
        }
        catch
        {
            throw new ArgumentException("Invalid date encoded in identity number.");
        }

        Console.WriteLine($"Fecha de nacimiento: {extractedDate}");

        return extractedDate;
    }
}
