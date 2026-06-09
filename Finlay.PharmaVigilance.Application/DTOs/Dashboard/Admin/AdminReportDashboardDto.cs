namespace Finlay.PharmaVigilance.Application.DTO;

public class AdminReportDashboardDto
{
    public int TotalReports { get; set; }
    public int Submitted { get; set; }

    public int UnderReview { get; set; }
    public int Reopened { get; set; }
    public int Approved { get; set; }
    public int Rejected { get; set; }
    public int Closed { get; set; }

    public IEnumerable<ProvinceReportStatusDto> Provinces { get; set; }
        = new List<ProvinceReportStatusDto>();

    public IEnumerable<SeverityLevelDistributionDto> SeverityDistribution { get; set; }
        = new List<SeverityLevelDistributionDto>();

    public IEnumerable<CausalityDistributionDto> CausalityDistribution { get; set; }
        = new List<CausalityDistributionDto>();

    public IEnumerable<SignificanceDistributionDto> SignificanceDistribution { get; set; }
        = new List<SignificanceDistributionDto>();

    public IEnumerable<MonthlyReportTrendDto> MonthlyTrends { get; set; }
        = new List<MonthlyReportTrendDto>();
}


public class MonthlyReportTrendDto
{
    public int Year { get; set; }

    public int Month { get; set; }

    public int TotalReports { get; set; }

}



public class ProvinceReportStatusDto
{
    public string ProvinceName { get; set; } = string.Empty;

    public int Submitted { get; set; }

    public int UnderReview { get; set; }

    public int Approved { get; set; }

    public int Rejected { get; set; }

    public int Closed { get; set; }

    public int Total { get; set; }
    public int Serious { get; set; }
}

public class SeverityLevelDistributionDto
{
    public string SeverityType { get; set; } = string.Empty;

    public int Count { get; set; }

    public double Percentage { get; set; }
}


public class CausalityDistributionDto
{
    public string Causality { get; set; } = string.Empty;

    public int Count { get; set; }

    public double Percentage { get; set; }
}



public class SignificanceDistributionDto
{
    public string Significance { get; set; } = string.Empty;

    public int Count { get; set; }

    public double Percentage { get; set; }
}



