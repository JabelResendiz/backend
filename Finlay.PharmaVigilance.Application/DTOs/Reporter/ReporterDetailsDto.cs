using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;


public class ReporterDetailsDto
{
    public required string FullName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
}

public class ReporterAdminDto : ReporterDetailsDto
{
    public required ReporterRelationship reporterRelationship { get; set; }
}