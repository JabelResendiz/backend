namespace Finlay.PharmaVigilance.Application.DTO;


public class ManufacturerResponseDto
{
    public required string Name { get; set; }
    public required Guid Id { get; set; }
    public required string Country { get; set; }
}