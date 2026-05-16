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

        var vaccinationKeys = new HashSet<(Guid VaccineId, DateTime Date)>();

        foreach (var vaccination in reportDto.Vaccinations)
        {
            if (!vaccination.AdministrationDate.HasValue)
            {
                throw new ArgumentException(
                    "Administration date is required",
                    nameof(vaccination.AdministrationDate));
            }

            var key = (vaccination.VaccineId, vaccination.AdministrationDate.Value.Date);

            if (!vaccinationKeys.Add(key))
            {
                throw new ArgumentException(
                    $"Duplicate vaccination found: Vaccine {vaccination.VaccineId} is already registered " +
                    $"for date {vaccination.AdministrationDate.Value:yyyy-MM-dd}. " +
                    "The same vaccine cannot be administered multiple times on the same day.",
                    nameof(reportDto.Vaccinations));
            }


            // Validate vaccine exists
            Console.WriteLine($"La vacuna es {vaccination.VaccineId}");

            var vaccine = await vaccineRepository.GetByIdAsync(vaccination.VaccineId)
                ?? throw new KeyNotFoundException(
                    $"Vaccine with ID {vaccination.VaccineId} not found in the database.");

            Console.WriteLine($"El lote es {vaccination.LotId}");

            var lot = await _unitOfWork.GetRepository<Lot>()
                        .GetByIdAsync(vaccination.LotId)
                        ?? throw new KeyNotFoundException(
                    $"Lot with ID {vaccination.LotId} not found in the database.");

            if (lot.VaccineId != vaccine.Id)
            {
                throw new ArgumentException(
                $"Lot {lot.Id} does not belong to vaccine {vaccine.Id}.",
                nameof(vaccination.LotId));
            }


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

            var vaccinationCenter = await _unitOfWork.GetRepository<VaccinationCenter>()
                                    .GetByIdAsync(vaccination.VaccinationCenterId)
                                    ?? throw new KeyNotFoundException(
                                        $"Vaccination Center with ID {vaccination.VaccinationCenterId} not found in the database.");

        }
    }
}
