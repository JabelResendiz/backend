using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;

public class VaccinatedSubject : GuidEntity
{
    public string FullName { get; set; } = null!;
    public string IdentityNumber { get; set; } = null!;

    public DateTime DateOfBirth => ExtractDateOfBirth(IdentityNumber);



    public Gender Gender { get; set; }
    public bool? IsPregnant { get; set; }

    public int ProvinceId { get; set; }
    public Province Province { get; set; } = null!;

    public int MunicipalityId { get; set; }
    public Municipality Municipality { get; set; } = null!;

    public string? HealthArea { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }


    public string? CurrentMedications { get; set; }
    public string? Allergies { get; set; }
    public string? MedicalHistory { get; set; }

    public ICollection<AefiReport> AefiReports { get; set; } = new List<AefiReport>();

    public int Age => CalculateAge(DateOfBirth);

    private static int CalculateAge(DateTime dateOfBirth)
    {


        var today = DateTime.Today;
        Console.WriteLine(today);
        int age = today.Year - dateOfBirth.Year;

        Console.WriteLine(today.AddYears(-age));

        if (dateOfBirth.Date > today.AddYears(-age))
        {
            age--;
        }

        return age;
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

        DateTime extractedDate;

        try
        {
            extractedDate = new DateTime(fullYear, month, day);
        }
        catch
        {
            throw new ArgumentException("Invalid date encoded in identity number.");
        }

        return extractedDate;
    }
}