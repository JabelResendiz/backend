namespace Finlay.PharmaVigilance.Domain.Entities;


public class VaccinationCenter : GuidEntity
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int MunicipalityId { get; set; }
    public int ProvinceId { get; set; }
    public Municipality Municipality { get; set; } = null!;
    public Province Province { get; set; } = null!;
}