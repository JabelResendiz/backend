namespace Finlay.PharmaVigilance.Application.DTO;

public class AdminVaccineDashboardDto
{
    public IEnumerable<VaccineStatusDto> Vaccines { get; set; }
        = new List<VaccineStatusDto>();

    public IEnumerable<SymptomDistributionDto> SymptomDistribution { get; set; }
        = new List<SymptomDistributionDto>();

}

public class VaccineStatusDto
{
    public string VaccineName { get; set; } = string.Empty;

    public int TotalReports { get; set; }

    public IEnumerable<LotsStatusDto> Lots { get; set; }
        = new List<LotsStatusDto>();
}


public class LotsStatusDto
{
    public string LotNumber { get; set; } = string.Empty;

    public int TotalReports { get; set; }
}


public class SymptomDistributionDto
{
    public string SymptomName { get; set; } = string.Empty;

    public int Count { get; set; }

    public double Percentage { get; set; }
}