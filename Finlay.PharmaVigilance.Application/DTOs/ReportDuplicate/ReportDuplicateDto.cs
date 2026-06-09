using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class ReportDuplicateDto
{
    public Guid Id { get; set; }
    public Guid AefiReportOriginalId { get; set; }
    public Guid AefiReportCopyId { get; set; }
    public string SubjectName { get; set; } = null!;
    public DateTime OriginalReportDate { get; set; }
    public DateTime CopyReportDate { get; set; }
    public EnumReportDuplicate EnumReportDuplicate { get; set; }
    public ReportStatus OriginalReportStatus { get; set; }


    public string? MedicalReviewerName { get; set; }
}