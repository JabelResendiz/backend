namespace Finlay.PharmaVigilance.Application.DTO;

public class ReportDetailDto
{
    public required DateTime ReportDate { get; set; }
    public required VaccinatedSubjectSummaryDto VaccinatedSubject { get; set; }
    public required ReporterDetailsDto Reporter { get; set; }
    public required IEnumerable<VaccinationDetailsDto> Vaccinations { get; set; }
    public required IEnumerable<AdverseEventDetailDto> AdverseEvents { get; set; }
}
