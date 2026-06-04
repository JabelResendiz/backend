
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class ReportPdfDto
{
    public required DateTime? ReportDate { get; set; }
    public required string NotificationNumber { get; set; }
    public required string Status { get; set; }
    public required string GlobalSeverityLevel { get; set; }
    public required VaccinatedSubjectPdfDto VaccinatedSubject { get; set; }
    public required List<VaccinationPdfDto> Vaccinations { get; set; }
    public required List<AdverseEventPdfDto> AdverseEvents { get; set; }
    public required ReporterPdfDto Reporter { get; set; }
    public string? Causality { get; set; }
    public string? ClinicalSignificance { get; set; }
    public DateTime? ReviewedAt { get; set; }
}

public class ReporterPdfDto
{
    public required string Name { get; set; }
    public required ReporterRelationship ReporterRelationship { get; set; }
    public required string ProvinceName { get; set; }
    public required string MunicipalityName { get; set; }
}

public class VaccinatedSubjectPdfDto
{
    public required string FullName { get; set; }
    public required int Age { get; set; }
    public required string ProvinceName { get; set; }
    public required string MunicipalityName { get; set; }
    public required Gender Gender { get; set; }
    public required bool IsPregnant { get; set; }
}

public class VaccinationPdfDto
{
    public required string VaccineName { get; set; }
    public required DateTime AdministrationDate { get; set; }
    public required string LotNumber { get; set; }

}

public class AdverseEventPdfDto
{
    public required DateTime StartDate { get; set; }
    public DateTime? FinishDate { get; set; }

    public string? Description { get; set; }
    public required bool VisitedDoctor { get; set; }

    public required bool WentToEmergencyRoom { get; set; }

    public required bool PermanentDisability { get; set; }

    public required bool WasHospitalized { get; set; }

    public required bool Anomaly { get; set; }

    public required bool NoComplications { get; set; }

    public required bool ResultedInDeath { get; set; }
    public DateTime? DeathDate { get; set; }
    public PatientStatus? CurrentStatus { get; set; }

    public Intensity? Intensity { get; set; }

    public SeverityLevel? SeverityLevel { get; set; }

    public required string SymptomName { get; set; }
}