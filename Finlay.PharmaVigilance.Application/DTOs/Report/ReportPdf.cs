

namespace Finlay.PharmaVigilance.Application.DTO;

public class ReportPdfDto
{
    public required DateTime? ReportDate { get; set; }
    public required VaccinatedSubjectPdfDto VaccinatedSubject { get; set; }
    public required List<VaccinationPdfDto> Vaccinations { get; set; }
    public required List<AdverseEventPdfDto> AdverseEvents { get; set; }
    public required ReporterPdfDto Reporter { get; set; }

}

public class ReporterPdfDto : ReporterDto
{

}

public class VaccinatedSubjectPdfDto : VaccinatedSubjectDto
{

}


public class VaccinationPdfDto : VaccinationDto
{
    public string VaccineName { get; set; } = string.Empty;
}

public class AdverseEventPdfDto : AdverseEventDto
{
    public List<string> SymptomsName { get; set; } = new();
}