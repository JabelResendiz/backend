using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;


public class ReportDuplicate : GuidEntity
{
    public EnumReportDuplicate EnumReportDuplicate { get; set; }
    public Guid AefiReportOriginalId { get; set; }
    public Guid AefiReportCopyId { get; set; }
    public Guid? ResolvedByUserId { get; set; }
    public DateTime? ResolvedAt { get; set; }

    public AefiReport AefiReportOriginal { get; set; } = null!;
    public AefiReport AefiReportCopy { get; set; } = null!;
}