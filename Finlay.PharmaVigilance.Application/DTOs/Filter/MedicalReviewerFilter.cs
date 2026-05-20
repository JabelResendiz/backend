namespace Finlay.PharmaVigilance.Application.DTO;


public class ReportMedicalReviewerFilter
{
    public string? VaccineName { get; set; }
    public string? Severity { get; set; }

    public string? SortBy { get; set; }   // "reportDate"
    public string? Order { get; set; }    // "asc" | "desc"
}