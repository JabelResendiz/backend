
namespace Finlay.PharmaVigilance.Application.DTO;

public class ReportSummaryDto
{
    public required DateTime ReportDate { get; set; }
    public required VaccinatedSubjectSummaryDto VaccinatedSubject { get; set; }
    public required IEnumerable<VaccinationSummaryDto> Vaccinations { get; set; }
    public required IEnumerable<AdverseEventSummaryDto> AdverseEvents { get; set; }
}
