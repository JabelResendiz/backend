using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class ResolveDuplicateDto
{
    public EnumReportDuplicate Verdict { get; set; }
}