using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class GetVaccineDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string TargetPathology { get; set; }
}

public class GetPrivateVaccineDto : GetVaccineDto
{
    public required bool IsActive { get; set; }
    public required VaccineType Type { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public required string Manufacturer { get; set; }
}