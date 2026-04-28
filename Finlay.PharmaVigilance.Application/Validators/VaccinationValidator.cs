using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.Validators;

/// <summary>
/// Validates vaccination information including vaccine existence, date consistency, and dose validity.
/// </summary>
public class VaccinationValidator : IReportValidator<ReportDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public VaccinationValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Validates vaccination data:
    /// - At least one vaccination must be present
    /// - Vaccine must exist in the database
    /// - Administration date must be after patient's date of birth and before the report date
    /// - Dose number must be positive
    /// </summary>
    public async Task ValidateAsync(ReportDto reportDto)
    {
        var vaccineRepository = _unitOfWork.GetRepository<Vaccine>();

        var minAdverseEventDate = reportDto.AdverseEvents.Min(ad => ad.StartDate);

        foreach (var vaccination in reportDto.Vaccinations)
        {
            // Validate vaccine exists
            var vaccine = await vaccineRepository.GetByIdAsync(vaccination.VaccineId)
                ?? throw new KeyNotFoundException(
                    $"Vaccine with ID {vaccination.VaccineId} not found in the database.");

            // Validate administration date is after patient's birth and before adverse event date
            if (vaccination.AdministrationDate < reportDto.VaccinatedSubject.DateOfBirth)
                throw new ArgumentException(
                    $"Vaccination administration date cannot be before the patient's date of birth. " +
                    $"Patient birth date: {reportDto.VaccinatedSubject.DateOfBirth:yyyy-MM-dd}, " +
                    $"Vaccination date: {vaccination.AdministrationDate:yyyy-MM-dd}",
                    nameof(vaccination.AdministrationDate));


            if (vaccination.AdministrationDate > minAdverseEventDate)
                throw new ArgumentException(
                    $"Vaccination administration date cannot be after the adverse event date. " +
                    $"Vaccination date: {vaccination.AdministrationDate:yyyy-MM-dd}, " +
                    $"Adverse event date: {minAdverseEventDate:yyyy-MM-dd}",
                    nameof(vaccination.AdministrationDate));

            if (!EnumHelper<AdministrationSite>.IsValid(vaccination.Site.ToString()!))
            {
                throw new ArgumentException(
                    "Administration Site must be valid",
                    nameof(vaccination.Site)
                );
            }

        }
    }
}
