namespace Finlay.PharmaVigilance.Domain.Entities;

public class Manufacturer : GuidEntity
{
    public string Name { get; set; } = null!;
    public string? Country { get; set; }

}