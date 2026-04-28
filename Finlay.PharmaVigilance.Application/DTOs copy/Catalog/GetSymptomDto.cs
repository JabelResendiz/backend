namespace Finlay.PharmaVigilance.Application.DTO;

public class GetSymptomDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
}

public class GetPrivateSymptomsDto : GetSymptomDto
{
    public required bool IsActive { get; set; }
    public required string Category { get; set; }
}