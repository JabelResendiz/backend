using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.Validators;

/// <summary>
/// Validates reporter information including age, location hierarchy, and professional requirements.
/// </summary>
public class ReporterValidator : IReportValidator<PublicAefiReportDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public ReporterValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Validates reporter data:
    /// - Date of birth must be before the report date and reporter must be at least 18 years old
    /// - Province must exist and municipality must belong to the reported province
    /// - If reporter relationship is HealthProfessional, professional license and institution are mandatory
    /// </summary>
    public async Task ValidateAsync(PublicAefiReportDto reportDto)
    {
        if (reportDto?.Reporter == null)
            throw new ArgumentNullException(nameof(reportDto.Reporter), "Reporter information is required.");

        var reporter = reportDto.Reporter;

        var minVaccinationDate = reportDto.Vaccinations.Min(v => v.AdministrationDate);

        // Validate date of birth
        if (reporter.DateOfBirth > minVaccinationDate)
            throw new ArgumentException(
                "Reporter's date of birth must be before the vaccination date.",
                nameof(reporter.DateOfBirth));

        var age = CalculateAge(reporter.DateOfBirth!.Value, reportDto.ReportDate!.Value);
        if (age < 18)
            throw new ArgumentException(
                "Reporter must be at least 18 years old.",
                nameof(reporter.DateOfBirth));

        // Validate province and municipality
        var province = await _unitOfWork.GetRepository<Province>()
            .GetByIdAsync(reporter.ProvinceId)
            ?? throw new KeyNotFoundException(
                $"Province with ID {reporter.ProvinceId} not found.");

        var municipality = await _unitOfWork.GetRepository<Municipality>()
            .GetByIdAsync(reporter.MunicipalityId)
            ?? throw new KeyNotFoundException(
                $"Municipality with ID {reporter.MunicipalityId} not found.");

        if (municipality.ProvinceId != province.Id)
            throw new ArgumentException(
                $"Municipality {municipality.Id} does not belong to province {province.Id}.",
                nameof(reporter.MunicipalityId));

        // Validate professional requirements if reporter is a health professional
        if (reporter.ReporterRelationship == ReporterRelationship.Doctor)
        {
            if (string.IsNullOrWhiteSpace(reporter.ProfessionalLicense))
                throw new ArgumentException(
                    "Professional license is required for health professionals.",
                    nameof(reporter.ProfessionalLicense));

            if (string.IsNullOrWhiteSpace(reporter.Institution))
                throw new ArgumentException(
                    "Institution is required for health professionals.",
                    nameof(reporter.Institution));
        }


        ValidateIdentityNumberFormat(reporter.IdentityNumber, reporter.DateOfBirth.Value);

        Console.WriteLine($"==========================={reporter.ReporterRelationship}=======================");

        if (!EnumHelper<ReporterRelationship>.IsValid(reporter.ReporterRelationship.ToString()!))
        {
            throw new ArgumentException(
                "Reporter Relationship must be valid",
                nameof(reporter.ReporterRelationship)
            );
        }
    }

    /// <summary>
    /// Calculates the age based on birth date and reference date.
    /// </summary>
    private static int CalculateAge(DateTime birthDate, DateTime referenceDate)
    {
        int age = referenceDate.Year - birthDate.Year;
        if (birthDate.Date > referenceDate.AddYears(-age))
            age--;
        return age;
    }



    /// <summary>
    /// Validates that the identity number is consistent with the date of birth.
    /// This is a basic validation that can be expanded based on specific requirements.
    /// </summary>
    private static void ValidateIdentityNumberFormat(string identityNumber, DateTime? dateOfBirth)
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

        if (extractedDate.Date != dateOfBirth?.Date)
        {
            throw new ArgumentException("Date of birth does not match identity number.");
        }

    }
}
