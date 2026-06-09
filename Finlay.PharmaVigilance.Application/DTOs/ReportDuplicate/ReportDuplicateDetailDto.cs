using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;


public class ReportDuplicateDetailDto
{
    public EnumReportDuplicate EnumReportDuplicate { get; set; }
    public ReportSectionResponsibleDto AefiReportOriginal { get; set; } = null!;
    public ReportSectionResponsibleDto AefiReportCopy { get; set; } = null!;
}