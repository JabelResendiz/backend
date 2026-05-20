namespace Finlay.PharmaVigilance.Application.DTO;

public class SectionResponsibleMunicipalDashboardDto
{
    public IEnumerable<VaccineStatsDto> TopVaccines { get; set; } = [];

    public IEnumerable<SymptomStatsDto> TopSymptoms { get; set; } = [];

    public IEnumerable<SeverityDistributionDto> SeverityDistribution { get; set; } = [];

    public IEnumerable<ReportsTimelineDto> ReportsTimeline { get; set; } = [];
    public int TotalDeaths { get; set; }
    public int TotalVisitedDoctor { get; set; }
    public int TotalEmergencyRoom { get; set; }
    public int TotalPermanentDisability { get; set; }
    public int TotalLifeThreatening { get; set; }

    //public IEnumerable<VaccinationCenterStatsDto> VaccinationCenters { get; set; } = [];
}

public class VaccineStatsDto
{
    public string VaccineName { get; set; } = string.Empty;

    public int TotalReports { get; set; }
}


public class SymptomStatsDto
{
    public string SymptomName { get; set; } = string.Empty;

    public int TotalReports { get; set; }
}


public class SeverityDistributionDto
{
    public string Severity { get; set; } = string.Empty;

    public int TotalReports { get; set; }
}



public class ReportsTimelineDto
{
    public string Label { get; set; } = string.Empty;

    public int TotalReports { get; set; }
}

