
namespace Finlay.PharmaVigilance.Application.DTO;


public class AdminPerformanceDashboardDto
{
    public int ActiveDoctors { get; set; }
    public double AvgReportsPerDoctor { get; set; }
    public double AvgReviewTimeHours { get; set; }
    public double AvgAssignmentHours { get; set; }

    public IEnumerable<ProvinceMedicalActivityDto> ActiveMedicalReviewers { get; set; }
        = new List<ProvinceMedicalActivityDto>();

}


public class PerformanceDto
{
    public int ActiveDoctors { get; set; }
    public double AvgReportsPerDoctor { get; set; }
    public double AvgReviewTimeHours { get; set; }
    public double AvgAssignmentHours { get; set; }
}



public class ProvinceMedicalActivityDto
{
    public string ProvinceName { get; set; } = string.Empty;

    public int ActiveDoctors { get; set; }
    public double AvgReportsPerDoctor { get; set; }
    public double AvgReviewTimeHours { get; set; }
    public double AvgAssignmentHours { get; set; }

    public IEnumerable<MunicipalityMedicalActivityDto> Municipalities { get; set; }
     = new List<MunicipalityMedicalActivityDto>();
}

public class MunicipalityMedicalActivityDto
{
    public string MunicipalityName { get; set; } = string.Empty;

    public int ActiveDoctors { get; set; }
    public double AvgReportsPerDoctor { get; set; }
    public double AvgReviewTimeHours { get; set; }
    public double AvgAssignmentHours { get; set; }
}

