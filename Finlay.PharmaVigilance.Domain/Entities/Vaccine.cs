
namespace Finlay.PharmaVigilance.Domain.Entities;

public class Vaccine : GenericEntity
{
    public string Name {get;set;} = null!;
    public string Manufacturer {get;set;} = null!;
    public string VaccineType {get;set;} = null!;
    public string Description {get;set;} = null!;
    public DateTime CreatedAt {get;set;}

}