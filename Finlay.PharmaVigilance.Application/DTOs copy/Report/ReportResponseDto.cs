using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class ReportDetailDto
{
    public required DateTime ReportDate { get; set; }
    public required VaccinatedSubjectResponseDto VaccinatedSubject { get; set; }
    public required ReporterResponseDto Reporter { get; set; }
    public required IEnumerable<VaccinationResponseDto> Vaccinations { get; set; }
    public required IEnumerable<AdverseEventDetailDto> AdverseEvents { get; set; }
}

public class VaccinatedSubjectResponseDto
{
    public required string FullName { get; set; }
}

public class ReporterResponseDto
{
    public required string FullName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
}

public class VaccinationResponseDto
{
    public required string VaccineName { get; set; }
    public required string BatchNumber { get; set; }
    public required AdministrationSite AdministrationSite { get; set; }
    public required int DoseNumber { get; set; }
    public required DateTime AdministrationDate { get; set; }
    public string? VaccinationCenter { get; set; }
}

public class AdverseEventDetailDto
{
    public required DateTime StartDate { get; set; }
    public required bool VisitedDoctor { get; set; }
    public required bool WentToEmergencyRoom { get; set; }
    public required bool PermanentDisability { get; set; }
    public required bool IsLifeThreatening { get; set; }
    public required bool ResultedInDeath { get; set; }
    public DateTime? DeathDate { get; set; }
    public required PatientStatus CurrentStatus { get; set; }
    public required IEnumerable<GetSymptomDto> Symptoms { get; set; }
}
