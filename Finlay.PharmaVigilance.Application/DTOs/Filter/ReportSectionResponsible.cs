namespace Finlay.PharmaVigilance.Application.DTO;


public class ReportSectionResponsibleFilter
{
    public string? VaccineName { get; set; }
    public Guid? VaccinationCenterId { get; set; }
    public string? Severity { get; set; }
    public string? ReportStatus { get; set; }

    public DateTime? From { get; set; }
    public DateTime? To { get; set; }


    public string? SortBy { get; set; }   // "reportDate"
    public string? Order { get; set; }    // "asc" | "desc"
}