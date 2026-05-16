namespace Finlay.PharmaVigilance.Domain.Entities;

public class Lot : GuidEntity
{
    public string LotNumber { get; set; } = null!;
    public Guid VaccineId { get; set; }
    public Vaccine Vaccine { get; set; } = null!;


}