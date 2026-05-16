using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;
using Microsoft.Extensions.Logging;

namespace Finlay.PharmaVigilance.Application.Validators;

/// <summary>
/// Validates adverse event information including symptom existence, date consistency, and death-related data.
/// </summary>
public class AdverseEventValidator : IReportValidator<ReportDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AdverseEventValidator> _logger;


    public AdverseEventValidator(IUnitOfWork unitOfWork, ILogger<AdverseEventValidator> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            // if (adverseEvent.Symptom == null || !adverseEvent.Symptoms.Any())
            //     throw new ArgumentException(
            //         "Each adverse event must have at least one symptom.",
            //         nameof(adverseEvent.Symptoms));

            if (!EnumHelper<PatientStatus>.IsValid(adverseEvent.CurrentStatus.ToString()!))
            {
                throw new ArgumentException(
                                "Patient Status must be valid",
                                nameof(adverseEvent.CurrentStatus)
                            );
            }

            if (!EnumHelper<Intensity>.IsValid(adverseEvent.Intensity.ToString()!))
            {
                throw new ArgumentException(
                                "Intensity must be valid",
                                nameof(adverseEvent.Intensity)
                            );
            }

            if (!EnumHelper<SeverityLevel>.IsValid(adverseEvent.SeverityLevel.ToString()!))
            {
                throw new ArgumentException(
                                "SeverityLevel must be valid",
                                nameof(adverseEvent.SeverityLevel)
                            );
            }

            // Validate symptom exist in database
            var symptom = await symptomsRepository.GetByIdAsync(adverseEvent.SymptomId)
                ?? throw new KeyNotFoundException(
                    $"Symptom with ID {adverseEvent.SymptomId} not found in the database.");



            // Validate event start date
            if (adverseEvent.StartDate < minVaccinationDate)
                throw new ArgumentException(
                    $"Adverse event start date cannot be before the vaccination date. " +
                    $"Earliest vaccination date: {minVaccinationDate:yyyy-MM-dd}, " +
                    $"Event start date: {adverseEvent.StartDate:yyyy-MM-dd}",
                    nameof(adverseEvent.StartDate));

            if (adverseEvent.StartDate > adverseEvent.FinishDate)
                throw new ArgumentException(
                    $"Adverse event finish date cannot be after the start date. " +
                    $"Event start date: {adverseEvent.StartDate:yyyy-MM-dd}, " +
                    $"Event finish date: {adverseEvent.FinishDate:yyyy-MM-dd}",
                    nameof(adverseEvent.FinishDate));

            if (adverseEvent.FinishDate > reportDto.ReportDate)
                throw new ArgumentException(
                    $"Adverse event finish date cannot be after the report date. " +
                    $"Event finish date: {adverseEvent.FinishDate:yyyy-MM-dd}, " +
                    $"Report date: {reportDto.ReportDate:yyyy-MM-dd}",
                    nameof(adverseEvent.FinishDate));



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
