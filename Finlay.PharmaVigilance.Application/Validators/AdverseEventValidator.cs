using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.Validators;

/// <summary>
/// Validates adverse event information including symptom existence, date consistency, and death-related data.
/// </summary>
public class AdverseEventValidator : IReportValidator<ReportDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public AdverseEventValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Validates adverse event data:
    /// - At least one adverse event must be present
    /// - Each adverse event must have at least one symptom
    /// - Symptoms must exist in the database
    /// - Event start date must be after vaccination and before report date
    /// - If death occurred, death date must be provided and fall between event date and report date
    /// </summary>
    public async Task ValidateAsync(ReportDto reportDto)
    {
        if (reportDto?.AdverseEvents == null || !reportDto.AdverseEvents.Any())
            throw new ArgumentException(
                "At least one adverse event is required.",
                nameof(reportDto.AdverseEvents));

        if (reportDto.Vaccinations == null || !reportDto.Vaccinations.Any())
            throw new ArgumentException(
                "At least one vaccination is required before adverse events can be reported.",
                nameof(reportDto.Vaccinations));

        var symptomsRepository = _unitOfWork.GetRepository<Symptom>();
        var minVaccinationDate = reportDto.Vaccinations.Min(v => v.AdministrationDate);

        foreach (var adverseEvent in reportDto.AdverseEvents)
        {
            // Validate symptoms exist
            if (adverseEvent.Symptoms == null || !adverseEvent.Symptoms.Any())
                throw new ArgumentException(
                    "Each adverse event must have at least one symptom.",
                    nameof(adverseEvent.Symptoms));

            if (!EnumHelper<PatientStatus>.IsValid(adverseEvent.CurrentStatus.ToString()!))
            {
                throw new ArgumentException(
                                "Patient Status must be valid",
                                nameof(adverseEvent.CurrentStatus)
                            );
            }

            // Validate each symptom exists in database
            foreach (var symptomId in adverseEvent.Symptoms)
            {
                var symptom = await symptomsRepository.GetByIdAsync(symptomId)
                    ?? throw new KeyNotFoundException(
                        $"Symptom with ID {symptomId} not found in the database.");

            }

            // Validate event start date
            if (adverseEvent.StartDate < minVaccinationDate)
                throw new ArgumentException(
                    $"Adverse event start date cannot be before the vaccination date. " +
                    $"Earliest vaccination date: {minVaccinationDate:yyyy-MM-dd}, " +
                    $"Event start date: {adverseEvent.StartDate:yyyy-MM-dd}",
                    nameof(adverseEvent.StartDate));

            if (adverseEvent.StartDate > reportDto.ReportDate)
                throw new ArgumentException(
                    $"Adverse event start date cannot be after the report date. " +
                    $"Event start date: {adverseEvent.StartDate:yyyy-MM-dd}, " +
                    $"Report date: {reportDto.ReportDate:yyyy-MM-dd}",
                    nameof(adverseEvent.StartDate));

            // Validate death-related information
            if (adverseEvent.ResultedInDeath)
            {
                if (!adverseEvent.DeathDate.HasValue)
                    throw new ArgumentException(
                        "Death date must be provided when 'Resulted In Death' is marked as true.",
                        nameof(adverseEvent.DeathDate));

                var deathDate = adverseEvent.DeathDate.Value;

                if (deathDate < adverseEvent.StartDate)
                    throw new ArgumentException(
                        $"Death date cannot be before the adverse event start date. " +
                        $"Event start date: {adverseEvent.StartDate:yyyy-MM-dd}, " +
                        $"Death date: {deathDate:yyyy-MM-dd}",
                        nameof(adverseEvent.DeathDate));

                if (deathDate > reportDto.ReportDate)
                    throw new ArgumentException(
                        $"Death date cannot be after the report date. " +
                        $"Death date: {deathDate:yyyy-MM-dd}, " +
                        $"Report date: {reportDto.ReportDate:yyyy-MM-dd}",
                        nameof(adverseEvent.DeathDate));
            }

            else
            {
                if (adverseEvent.DeathDate.HasValue)
                    throw new ArgumentException(
                        "Death date dont must be provided when 'Resulted In Death' is marked as false.",
                        nameof(adverseEvent.DeathDate));

            }
        }
    }
}
