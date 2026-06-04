using Finlay.PharmaVigilance.Application.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class ReportPdfQueryDto
{
    public required string NotificationNumber { get; set; }
    public ReportPdfTemplateType TemplateType { get; set; } = ReportPdfTemplateType.Admin;
}
