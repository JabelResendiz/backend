using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.Helpers;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.Validators;

/// <summary>
/// Validates vaccinated subject (patient) information including age, location, pregnancy status, and identity validation.
/// </summary>
public class VaccinatedSubjectValidator : IReportValidator<ReportDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public VaccinatedSubjectValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Validates vaccinated subject data:
    /// - Date of birth must be before the report date
    /// - Province must exist and municipality must belong to the reported province
    /// - If subject is female, IsPregnant status must be explicitly set
    /// - Identity number format must be consistent with date of birth
    /// </summary>
    public async Task ValidateAsync(ReportDto reportDto)
    {
        if (reportDto?.VaccinatedSubject == null)
            throw new ArgumentNullException(nameof(reportDto.VaccinatedSubject), "Vaccinated subject information is required.");

        var subject = reportDto.VaccinatedSubject;

        var minVaccinationDate = reportDto.Vaccinations.Min(v => v.AdministrationDate);

        var dateOfBirth = ExtractDateHelper.ExtractDateOfBirht(subject.IdentityNumber);

        // Validate date of birth
        if (dateOfBirth > minVaccinationDate)
            throw new ArgumentException(
                "Vaccinated subject's date of birth must be before the vaccination date.",
                nameof(dateOfBirth));

        // Validate province and municipality
        var province = await _unitOfWork.GetRepository<Province>()
            .GetByIdAsync(subject.ProvinceId)
            ?? throw new KeyNotFoundException(
                $"Province with ID {subject.ProvinceId} not found.");

        var municipality = await _unitOfWork.GetRepository<Municipality>()
            .GetByIdAsync(subject.MunicipalityId)
            ?? throw new KeyNotFoundException(
                $"Municipality with ID {subject.MunicipalityId} not found.");

        if (municipality.ProvinceId != province.Id)
            throw new ArgumentException(
                $"Municipality {municipality.Id} does not belong to province {province.Id}.",
                nameof(subject.MunicipalityId));

        // Validate pregnancy status for females
        if (subject.Gender == Gender.Female && !subject.IsPregnant.HasValue)
            throw new ArgumentException(
                "IsPregnant status must be explicitly set for female subjects.",
                nameof(subject.IsPregnant));

        // Validate identity number consistency with date of birth
        ValidateIdentityNumberFormat(subject.IdentityNumber, dateOfBirth);

        if (!EnumHelper<Gender>.IsValid(subject.Gender.ToString()!))
        {
            throw new ArgumentException(
                "Vaccinated Subject's Gender must be valid",
                nameof(subject.Gender)
            );
        }
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
